using System;
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

            //CurrentChannel =
            //    _timetableService.Channels.Single(w => w.ChannelId == _configuration.Root.LastViewedChannelStr);
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
                    // ReSharper disable once ReplaceWithSingleCallToFirstOrDefault
                    // First もなにも、1つしか無いはず。
                    currentSlot = connection.Slots.AsNoTracking()
                                            .Where(w => w.Channel.ChannelId == CurrentChannel.ChannelId)
                                            .Where(w => w.StartAt <= DateTime.Now && DateTime.Now <= w.EndAt)
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
                    CurrentSlot = currentSlot;
                    //var currentDetail = await _abemaApiHost.CurrentSlot(currentSlot.SlotId);
                    //CurrentSlot = currentDetail?.Id == null ? currentSlot : currentDetail;
                }
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