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
        private IDisposable _disposable;

        public AbemaState()
        {
            CurrentChannel = Configuration.Instance.Root.LastViewedChannel;
            _disposable = Observable.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(1)).Subscribe(async w => await Sync());
        }

        ~AbemaState()
        {
            _disposable.Dispose();
        }

        public void OnChannelChanged(string url)
        {
            _disposable.Dispose();
            CurrentChannel = AbemaChannelExt.FromUrlString(url);
            _disposable = Observable.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(1)).Subscribe(async w => await Sync());
        }

        private async Task Sync()
        {
            if (Timetable.Instance.LastSyncTime.Day != DateTime.Now.Day)
                await Timetable.Instance.Sync();
            var media = Timetable.Instance.Media;
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