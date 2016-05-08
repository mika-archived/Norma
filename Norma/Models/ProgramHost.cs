using System;
using System.Linq;
using System.Reactive.Linq;

using Prism.Mvvm;

namespace Norma.Models
{
    internal class ProgramHost : BindableBase, IDisposable
    {
        private AbemaChannels _channel;
        private IDisposable _disposable;

        #region Implementation of IDisposable

        public void Dispose()
        {
            _disposable?.Dispose();
        }

        #endregion

        public void OnChannelChanged(AbemaChannels channel)
        {
            _channel = channel;
            _disposable?.Dispose();

            _disposable = Observable.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(10))
                                    .Subscribe(w => FetchProgramInfo());
        }

        private void FetchProgramInfo()
        {
            var ts = Timetable.Instance.Media;
            var currenSchedule = ts.ChannelSchedules.First(w => w.ChannelId == _channel.ToUrlString()); // 今日
            var currentProgram = currenSchedule.Slots.Single(w => w.StartAt <= DateTime.Now && w.EndAt >= DateTime.Now);
            Title = currentProgram.Title;
            Description = currentProgram.DetailHighlight;
        }

        #region HasInfo

        private bool _hasInfo;

        public bool HasInfo
        {
            get { return _hasInfo; }
            set { SetProperty(ref _hasInfo, value); }
        }

        #endregion

        #region Title

        private string _title;

        public string Title
        {
            get { return _title; }
            set
            {
                if (SetProperty(ref _title, value))
                    HasInfo = !string.IsNullOrWhiteSpace(value);
            }
        }

        #endregion

        #region Description

        private string _description;

        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }

        #endregion

        #region Thumb1

        private string _thumb1;

        public string Thumb1
        {
            get { return _thumb1; }
            set { SetProperty(ref _thumb1, value); }
        }

        #endregion
    }
}