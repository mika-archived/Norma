using System;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

using Norma.Gamma.Models;

using Prism.Mvvm;

namespace Norma.Models
{
    internal class AbemaState : BindableBase
    {
        private readonly Configuration _configuration;
        private readonly Timetable _timetable;
        private IDisposable _disposable;

        public AbemaState(Configuration configuration, Timetable timetable)
        {
            _configuration = configuration;
            _timetable = timetable;
            CurrentChannel = _configuration.Root.LastViewedChannel;
            _disposable = Observable.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(1)).Subscribe(async w => await Sync());
        }

        ~AbemaState()
        {
            _disposable.Dispose();
        }

        public void OnChannelChanged(string url)
        {
            _disposable.Dispose();
            _configuration.Root.LastViewedChannel = CurrentChannel = AbemaChannelExt.FromUrlString(url);
            _disposable = Observable.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(1)).Subscribe(async w => await Sync());
        }

        private async Task Sync()
        {
            if (_timetable.LastSyncTime.Day != DateTime.Now.Day)
                await _timetable.Sync();
            var media = _timetable.Media;
            var schedule = media.ChannelSchedules.First(w => w.ChannelId == CurrentChannel.ToUrlString());
            CurrentSlot = schedule.Slots.SingleOrDefault(w => w.StartAt <= DateTime.Now && DateTime.Now <= w.EndAt);
            if (CurrentSlot == null)
            {
                CurrentProgram = null;
                return;
            }

            var perTime = (CurrentSlot.EndAt - CurrentSlot.StartAt).TotalSeconds / CurrentSlot.Programs.Length;
            var count = 0;
            while (!(CurrentSlot.StartAt.AddSeconds(perTime * count) <= DateTime.Now &&
                     DateTime.Now <= CurrentSlot.StartAt.AddSeconds(perTime * ++count))) {}

            CurrentProgram = CurrentSlot.Programs[--count];
        }

        #region CurrentChannel

        private AbemaChannel _currentChannel;

        public AbemaChannel CurrentChannel
        {
            get { return _currentChannel; }
            private set { SetProperty(ref _currentChannel, value); }
        }

        #endregion

        #region IsBroadcastCm

        private bool _isBroadcastCm;

        public bool IsBroadcastCm
        {
            get { return _isBroadcastCm; }
            set { SetProperty(ref _isBroadcastCm, value); }
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

        #region CurrentProgram

        private Program _currentProgram;

        public Program CurrentProgram
        {
            get { return _currentProgram; }
            private set { SetProperty(ref _currentProgram, value); }
        }

        #endregion
    }
}