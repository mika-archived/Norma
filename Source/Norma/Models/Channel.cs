using System;
using System.Reactive.Linq;

using Norma.Eta.Extensions;
using Norma.Eta.Models;
using Norma.Eta.Properties;

using Prism.Mvvm;

namespace Norma.Models
{
    internal class Channel : BindableBase, IDisposable
    {
        private readonly IDisposable _disposable;
        private readonly Timetable _timetable;
        public string ChannelType { get; }
        public string LogoUrl { get; private set; }

        public Channel(string channel, Configuration configuration, Timetable timetable)
        {
            ChannelType = channel;
            _timetable = timetable;
            LogoUrl = $"https://hayabusa.io/abema/channels/logo/{ChannelType}.w120.png";

            // 1分毎にサムネとか更新
            var val = configuration.Root.Operation.UpdateIntervalOfThumbnails;
            _disposable = Observable.Timer(TimeSpanExt.OneSecond, TimeSpan.FromSeconds(val))
                                    .Subscribe(w => UpdateChannelInfo());
        }

        #region Implementation of IDisposable

        public void Dispose()
        {
            _disposable?.Dispose();
        }

        #endregion

        private void UpdateChannelInfo()
        {
            var currentSlot = _timetable.CurrentSlot(ChannelType);
            if (currentSlot != null)
            {
                Title = currentSlot.Title;
                StartAt = currentSlot.StartAt;
                EndAt = currentSlot.EndAt;
            }
            var date = DateTime.Now;
            if (date.Second % 10 != 0)
                date = date.AddSeconds(-(date.Second % 10)); // サムネイルが10秒に発行されるので、N % 10 == 0秒に修正する
            var time = date.ToString("yyyyMMddHHmmss");
            ThumbnailUrl = $"https://hayabusa.io/abema/channels/time/{time}/{ChannelType}.w132.h75.png";
            StatusInfo.Instance.Text = Resources.ReloadingThumbnail;
        }

        #region ThumbnailUrl

        private string _thumbnailUrl;

        public string ThumbnailUrl
        {
            get { return _thumbnailUrl; }
            set { SetProperty(ref _thumbnailUrl, value); }
        }

        #endregion

        #region Title

        private string _title;

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        #endregion

        #region StartAt

        private DateTime _startAt;

        public DateTime StartAt
        {
            get { return _startAt; }
            set { SetProperty(ref _startAt, value); }
        }

        #endregion

        #region EndAt

        private DateTime _endAt;

        public DateTime EndAt
        {
            get { return _endAt; }
            set { SetProperty(ref _endAt, value); }
        }

        #endregion
    }
}