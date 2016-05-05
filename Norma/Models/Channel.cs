using System;
using System.Reactive.Linq;

using Prism.Mvvm;

namespace Norma.Models
{
    internal class Channel : BindableBase, IDisposable
    {
        private readonly IDisposable _disposable;
        public AbemaChannels ChannelType { get; }
        public string LogoUrl { get; private set; }

        public Channel(AbemaChannels channel)
        {
            ChannelType = channel;
            LogoUrl = $"https://hayabusa.io/abema/channels/logo/{ChannelType.ToUrlString()}.w120.png";

            UpdateThumbnail();
            _disposable = Observable.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(60)).Subscribe(w => UpdateThumbnail());
                // 1分毎にサムネ更新
        }

        #region Implementation of IDisposable

        public void Dispose()
        {
            _disposable?.Dispose();
        }

        #endregion

        private void UpdateThumbnail()
        {
            var channel = ChannelType.ToUrlString();
            var date = DateTime.Now;
            if (date.Second % 10 != 0)
                date = date.AddSeconds(-(date.Second % 10)); // サムネイルが10秒に発行されるので、N % 10 == 0秒に修正する
            var time = date.ToString("yyyyMMddhhmmss");
            ThumbnailUrl = $"https://hayabusa.io/abema/channels/time/{time}/{channel}.w132.h75.png";
        }

        #region ThumbnailUrl

        private string _thumbnailUrl;

        public string ThumbnailUrl
        {
            get { return _thumbnailUrl; }
            set { SetProperty(ref _thumbnailUrl, value); }
        }

        #endregion
    }
}