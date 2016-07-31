using System;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

using Norma.Delta.Models;
using Norma.Delta.Services;
using Norma.Eta.Models;
using Norma.Eta.Models.Enums;

using Prism.Mvvm;

namespace Norma.Models
{
    internal class AbemaState : BindableBase
    {
        private readonly AbemaApiClient _abemaApiHost;
        private readonly Configuration _configuration;
        private readonly TimetableService _timetableService;
        private IDisposable _disposable;

        public AbemaState(AbemaApiClient abemaApiHost, Configuration configuration, TimetableService timetableService)
        {
            _abemaApiHost = abemaApiHost;
            _configuration = configuration;
            _timetableService = timetableService;
        }

        public void Start()
        {
            CurrentChannel = _timetableService.Channels.Single(w => w.Id == _configuration.Root.LastViewedChannelStr);
            var val = _configuration.Root.Operation.UpdateIntervalOfProgram;
            _disposable =
                Observable.Timer(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(val)).Subscribe(async w => await Sync());
        }

        ~AbemaState()
        {
            _disposable.Dispose();
        }

        public void OnChannelChanged(string url)
        {
            _disposable.Dispose();
            CurrentChannel = _timetable.Channels?.Single(w => w.Id == AbemaChannelExt.ToIdentifier(url));
            if (CurrentChannel != null)
                _configuration.Root.LastViewedChannelStr = CurrentChannel.Id;
            var val = _configuration.Root.Operation.UpdateIntervalOfProgram;
            _disposable = Observable.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(val)).Subscribe(async w => await Sync());
        }

        private async Task Sync()
        {
            try
            {
                var schedule = _timetable.ChannelSchedules.FirstOrDefault(w => w.ChannelId == CurrentChannel.Id);
                if (schedule == null)
                {
                    CurrentSlot = null;
                    CurrentEpisode = null;
                    return;
                }
                var currentSlot =
                    schedule.Slots.SingleOrDefault(w => w.StartAt <= DateTime.Now && DateTime.Now <= w.EndAt);
                if (currentSlot == null)
                {
                    CurrentSlot = null;
                    CurrentEpisode = null;
                    return;
                }
                if (CurrentSlot?.Id != currentSlot.Id)
                {
                    var currentDetail = await _abemaApiHost.CurrentSlot(currentSlot.Id);
                    CurrentSlot = currentDetail?.Id == null ? currentSlot : currentDetail;
                }
                // ReSharper disable once PossibleNullReferenceException
                var perTime = (CurrentSlot.EndAt - CurrentSlot.StartAt).TotalSeconds / CurrentSlot.Programs.Length;
                var count = 0;
                while (!(CurrentSlot.StartAt.AddSeconds(perTime * count) <= DateTime.Now &&
                         DateTime.Now <= CurrentSlot.StartAt.AddSeconds(perTime * ++count))) {}

                --count;
                if (count < 0 || count >= CurrentSlot.Programs.Length)
                    return;
                CurrentEpisode = CurrentSlot.Programs[count];
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