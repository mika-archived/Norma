using System;
using System.Linq;
using System.Reactive.Linq;

using Microsoft.Practices.ServiceLocation;

using Norma.Delta.Models;
using Norma.Eta.Extensions;
using Norma.Eta.Models;
using Norma.Eta.Properties;
using Norma.Eta.Services;

using Prism.Mvvm;

namespace Norma.Models
{
    internal class AbemaChannel : BindableBase, IDisposable
    {
        private readonly IDisposable _disposable;
        private readonly StatusService _statusService;
        private readonly TimetableService _timetableService;
        public Channel Channel { get; }
        public string LogoUrl { get; private set; }

        public AbemaChannel(Channel channel)
        {
            Channel = channel;
            _statusService = ServiceLocator.Current.GetInstance<StatusService>();
            _timetableService = ServiceLocator.Current.GetInstance<TimetableService>();
            LogoUrl = $"https://hayabusa.io/abema/channels/logo/{Channel.ChannelId}.w120.png";

            // 1分毎にサムネとか更新
            var configuration = ServiceLocator.Current.GetInstance<Configuration>();
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
            var currentSlot = _timetableService.CurrentSlots.SingleOrDefault(w => w.Channel.ChannelId == Channel.ChannelId);
            if (currentSlot != null)
            {
                Title = currentSlot.Title;
                StartAt = currentSlot.StartAt;
                EndAt = currentSlot.EndAt;
            }
            var date = DateTime.Now;
            if (date.Second % 10 != 0)
                date = date.AddSeconds(-(date.Second % 10)); // サムネイルが10秒単位で発行されるので、N % 10 == 0秒に修正する
            var time = date.ToString("yyyyMMddHHmmss");
            ThumbnailUrl = $"https://hayabusa.io/abema/channels/time/{time}/{Channel.ChannelId}.w132.h75.png";
            _statusService.UpdateStatus(Resources.ReloadingThumbnail);
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