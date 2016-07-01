using System;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

using Norma.Eta.Models;
using Norma.Gamma.Models;

using Prism.Mvvm;

namespace Norma.Models
{
    internal class AbemaState : BindableBase
    {
        private readonly AbemaApiHost _abemaApiHost;
        private readonly Configuration _configuration;
        private readonly Timetable _timetable;
        private IDisposable _disposable;

        public AbemaState(AbemaApiHost abemaApiHost, Configuration configuration, Timetable timetable)
        {
            _abemaApiHost = abemaApiHost;
            _configuration = configuration;
            _timetable = timetable;
            CurrentChannel = configuration.Root.LastViewedChannel;
            // IsBroadcastCm = true;
        }

        public void Start()
        {
            var val = _configuration.Root.Operation.UpdateIntervalOfProgram;
            _disposable = Observable.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(val)).Subscribe(async w => await Sync());
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
            _disposable = Observable.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(val)).Subscribe(async w => await Sync());
        }

        private async Task Sync()
        {
            var schedule = _timetable.ChannelSchedules.First(w => w.ChannelId == CurrentChannel.ToUrlString());
            var currentSlot = schedule.Slots.SingleOrDefault(w => w.StartAt <= DateTime.Now && DateTime.Now <= w.EndAt);
            if (currentSlot == null)
            {
                CurrentProgram = null;
                return;
            }
            var currentDetail = await _abemaApiHost.CurrentSlot(currentSlot.Id);
            CurrentSlot = currentDetail?.Id == null ? currentSlot : currentDetail;
            var perTime = (CurrentSlot.EndAt - CurrentSlot.StartAt).TotalSeconds / CurrentSlot.Programs.Length;
            var count = 0;
            while (!(CurrentSlot.StartAt.AddSeconds(perTime * count) <= DateTime.Now &&
                     DateTime.Now <= CurrentSlot.StartAt.AddSeconds(perTime * ++count))) {}

            --count;
            if (count < 0 || count >= CurrentSlot.Programs.Length)
                return;
            CurrentProgram = CurrentSlot.Programs[count];
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