using System;
using System.Linq;
using System.Reactive.Linq;

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
            CurrentChannel = configuration.Root.LastViewedChannel;
            // IsBroadcastCm = true;
        }

        public void Start()
        {
            var val = _configuration.Root.Operation.UpdateIntervalOfProgram;
            _disposable = Observable.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(val)).Subscribe(w => Sync());
        }

        ~AbemaState()
        {
            _disposable.Dispose();
        }

        public void OnChannelChanged(string url)
        {
            _disposable.Dispose();
            _configuration.Root.LastViewedChannel = CurrentChannel = AbemaChannelExt.FromUrlString(url);
            var val = _configuration.Root.Operation.UpdateIntervalOfProgram;
            _disposable = Observable.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(val)).Subscribe(w => Sync());
        }

        private void Sync()
        {
            var schedule = _timetable.ChannelSchedules.First(w => w.ChannelId == CurrentChannel.ToUrlString());
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

        [Obsolete("2016/05/24 17:10 ~のバグ/仕様変更？で、ちょっとおかしくなる。")]
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