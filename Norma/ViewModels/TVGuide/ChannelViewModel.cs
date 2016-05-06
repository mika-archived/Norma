using Norma.Extensions;
using Norma.Helpers;
using Norma.Models;
using Norma.ViewModels.Internal;

namespace Norma.ViewModels.TVGuide
{
    internal class ChannelViewModel : ViewModel
    {
        private readonly Channel _model;
        private readonly ShellViewModel _parentViewModel;

        public string LogoUrl => _model.LogoUrl;

        public ChannelViewModel(ShellViewModel parentViewModel, Channel channel)
        {
            _parentViewModel = parentViewModel;
            _model = channel;

            channel.Subscribe(nameof(Channel.ThumbnailUrl), w => ThumbnailUrl = channel.ThumbnailUrl).AddTo(this);
        }

        // CallMethodAction
        public void ChannelClick()
        {
            _parentViewModel.HostViewModel.Address = $"https://abema.tv/now-on-air/{_model.ChannelType.ToUrlString()}";
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