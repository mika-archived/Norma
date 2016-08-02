using System;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

using Norma.Delta.Models;
using Norma.Delta.Services;
using Norma.Eta.Models;
using Norma.Eta.Services;

using Prism.Mvvm;

namespace Norma.Models
{
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class AbemaState : BindableBase
    {
        private readonly AbemaApiClient _abemaApiHost;
        private readonly Configuration _configuration;
        private readonly DatabaseService _databaseService;
        private readonly IDisposable _disposable;
        private readonly TimetableService _timetableService;

        public AbemaState(AbemaApiClient abemaApiHost, Configuration configuration, DatabaseService databaseService,
                          TimetableService timetableService)
        {
            _abemaApiHost = abemaApiHost;
            _configuration = configuration;
            _databaseService = databaseService;
            _timetableService = timetableService;
            var interval = TimeSpan.FromSeconds(configuration.Root.Operation.UpdateIntervalOfProgram);

            using (var connector = databaseService.Connect())
                CurrentChannel = connector.Channels.Single(w => w.ChannelId == _configuration.Root.LastViewedChannelStr);
            _disposable = Observable.Timer(TimeSpan.FromSeconds(1), interval).Subscribe(async w => await Sync());
        }

        ~AbemaState()
        {
            _disposable.Dispose();
        }

        private async Task Sync()
        {
            try
            {
                Slot currentSlot;
                using (var connection = _databaseService.Connect())
                {
                    var datetime = DateTime.Now;

                    // このクエリだけ Lazy Loading を Off にしておく(SQLite だと Include 連結できない)
                    connection.TurnOffLazyLoading();
                    // ReSharper disable once ReplaceWithSingleCallToFirstOrDefault
                    currentSlot = connection.Slots.AsNoTracking()
                                            .Where(w => w.Channel.ChannelId == CurrentChannel.ChannelId)
                                            .Where(w => w.StartAt <= datetime && datetime <= w.EndAt)
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
                        CurrentSlot = currentSlot;
                        // DB の更新
                        // using (var connection = _databaseService.Connect()) {}
                    }
                    else
                    {
                        var episode = currentSlot.Episodes.First();
                        using (var connection = _databaseService.Connect())
                        {
                            var dbEpisode = connection.Episodes.Single(w => w.EpisodeId == episode.EpisodeId);
                            episode.Casts = dbEpisode.Casts.ToList();
                            episode.Crews = dbEpisode.Crews.ToList();
                            episode.Thumbnails = dbEpisode.Thumbnails.ToList();
                        }
                        CurrentSlot = currentSlot;
                        CurrentEpisode = episode;
                    }
                }
                // データの更新と、取得
                // ReSharper disable once PossibleNullReferenceException
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
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

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