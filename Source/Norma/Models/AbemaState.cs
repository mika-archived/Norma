using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

using Norma.Delta.Services;
using Norma.Eta.Extensions;
using Norma.Eta.Filters;
using Norma.Eta.Helpers;
using Norma.Eta.Models;
using Norma.Eta.Services;
using Norma.Gamma.Models;

using Prism.Mvvm;

using Channel = Norma.Delta.Models.Channel;
using Episode = Norma.Delta.Models.Episode;
using Slot = Norma.Delta.Models.Slot;

namespace Norma.Models
{
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class AbemaState : BindableBase, INetworkCaptureRequestAware
    {
        private readonly AbemaApiClient _abemaApiHost;
        private readonly Configuration _configuration;
        private readonly DatabaseService _databaseService;
        private readonly IDisposable _disposable;

        private readonly List<IFilter> _filters = new List<IFilter>
        {
            new MBToSBFilter(),
            new CopyrightFilter(),
            new RoleFilter(),
            new InvalidBracesFilter(),
            new BracesFilter(),
            new SpaceFilter(),
            new EmptyFilter(),
            new SeparatorFilter()
        };

        private readonly TimetableService _timetableService;
        private string _slotId = "";

        public AbemaState(AbemaApiClient abemaApiHost, Configuration configuration, DatabaseService databaseService,
                          NetworkHandler networkHandler, TimetableService timetableService)
        {
            _abemaApiHost = abemaApiHost;
            _configuration = configuration;
            _databaseService = databaseService;
            _timetableService = timetableService;
            var interval = TimeSpan.FromSeconds(configuration.Root.Operation.UpdateIntervalOfProgram);

            using (var connector = databaseService.Connect())
                CurrentChannel = connector.Channels.Single(w => w.ChannelId == _configuration.Root.LastViewedChannelStr);
            _disposable = Observable.Timer(TimeSpan.Zero, interval).Subscribe(w => SyncEpisode());
            networkHandler.RegisterInstance(this, w => w.Url.StartsWith("https://api.abema.io/v1/slotAudience?"));
        }

        #region Implementation of INetworkCaptureRequestAware

        public void OnRequestHandling(NetworkEventArgs e)
        {
            var slotId = UrlHelper.ParseQuery(e.Url)["slotId"];
            if (_slotId == slotId)
                return;
            _slotId = slotId;
            Observable.Return(0).Subscribe(async w => await SyncProgram());
        }

        #endregion

        ~AbemaState()
        {
            _disposable.Dispose();
        }

        private async Task SyncProgram()
        {
            try
            {
                Slot currentSlot;
                using (var connection = _databaseService.Connect())
                {
                    // このクエリだけ Lazy Loading を Off にしておく(SQLite だと Include 連結できない)
                    connection.TurnOffLazyLoading();
                    // ReSharper disable once ReplaceWithSingleCallToFirstOrDefault
                    currentSlot = connection.Slots.AsNoTracking()
                                            .Where(w => w.SlotId == _slotId)
                                            .Include(w => w.Episodes)
                                            .FirstOrDefault();
                }
                if (currentSlot == null)
                {
                    CurrentSlot = null;
                    CurrentEpisode = null;
                    return;
                }

                if (CurrentSlot?.SlotId != currentSlot.SlotId)
                {
                    var currentDetail = await _abemaApiHost.CurrentSlot(currentSlot.SlotId);
                    if (currentSlot.Episodes.Count != currentDetail.Programs.Length)
                    {
                        // Ep.2 ~ を更新
                        UpdateEpisode(currentSlot, currentDetail.Programs);
                        CurrentSlot = currentSlot;
                    }
                    // キャスト情報などを更新
                    using (var connection = _databaseService.Connect())
                    {
                        foreach (var episode in currentSlot.Episodes)
                        {
                            var dbEpisode = connection.Episodes.Single(w => w.EpisodeId == episode.EpisodeId);
                            episode.Casts = dbEpisode.Casts.ToList();
                            episode.Crews = dbEpisode.Crews.ToList();
                            episode.Thumbnails = dbEpisode.Thumbnails.ToList();
                        }
                    }
                    CurrentSlot = currentSlot;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        private void SyncEpisode()
        {
            if (CurrentSlot == null)
                return;
            var episodes = CurrentSlot.Episodes.Count;
            var perTime = (CurrentSlot.EndAt - CurrentSlot.StartAt).TotalSeconds / episodes;
            var count = 0;
            while (!(CurrentSlot.StartAt.AddSeconds(perTime * count) <= DateTime.Now &&
                     DateTime.Now <= CurrentSlot.StartAt.AddSeconds(perTime * ++count))) {}

            --count;
            if (count < 0 || count >= episodes)
                return;
            CurrentEpisode = CurrentSlot.Episodes.Skip(count).First();
        }

        private void UpdateEpisode(Slot slot, Program[] programs)
        {
            using (var connection = _databaseService.Connect())
            {
                var episodes = new List<Episode>();
                var episodeList = connection.Episodes.ToList();
                var castList = connection.Casts.ToList();
                var copyRightList = connection.Copyrights.ToList();
                var crewList = connection.Crews.ToList();
                var seriesList = connection.Series.ToList();
                foreach (var rawEpisode in programs.Skip(1))
                {
                    var episode = rawEpisode.ConvertToEpisode();
                    if (rawEpisode.Credit.Cast != null)
                        foreach (var cast in rawEpisode.Credit.Cast.Select(Filter))
                            episode.Casts.Add(castList.Single(w => w.Name == cast));
                    if (rawEpisode.Credit.Copyrights != null)
                        foreach (var copyright in rawEpisode.Credit.Copyrights.Select(Filter))
                            episode.Copyrights.Add(copyRightList.Single(w => w.Name == copyright));
                    if (rawEpisode.Credit.Crews != null)
                        foreach (var crew in rawEpisode.Credit.Crews.Select(Filter))
                            episode.Crews.Add(crewList.Single(w => w.Name == crew));
                    foreach (var thumbnail in rawEpisode.ProvidedInfo.ConvertToThumbnail())
                        episode.Thumbnails.Add(thumbnail);
                    episode.Series = seriesList.SingleOrDefault(w => w.SeriesId == rawEpisode.Series.Id);
                    episodes.Add(episode);
                }
                episodes.ForEach(w => episodeList.AddIfNotExists(connection.Episodes, w, v => v.EpisodeId == w.EpisodeId));
                episodes.ForEach(w => connection.Slots.Single(v => v.SlotId == slot.SlotId).Episodes.Add(w));
                connection.DetectChanges();
                connection.SaveChanges();

                episodes.ForEach(w => slot.Episodes.Add(w));
            }
        }

        private string Filter(string str) => _filters.Aggregate(str, (current, filter) => filter.Call(current));

        #region CurrentChannel

        private Channel _currentChannel;

        public Channel CurrentChannel
        {
            get { return _currentChannel; }
            set { SetProperty(ref _currentChannel, value); }
        }

        #endregion

        #region CurrentSlot

        private Slot _currentSlot;

        public Slot CurrentSlot
        {
            get { return _currentSlot; }
            private set { SetProperty(ref _currentSlot, value); }
        }

        #endregion

        #region CurrentEpisode

        private Episode _currentEpisode;

        public Episode CurrentEpisode
        {
            get { return _currentEpisode; }
            private set { SetProperty(ref _currentEpisode, value); }
        }

        #endregion
    }
}