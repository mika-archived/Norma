using Norma.Extensions;
using Norma.Models;
using Norma.ViewModels.Internal;

using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace Norma.ViewModels.TVGuide
{
    internal class ChannelViewModel : ViewModel
    {
        private readonly Channel _model;
        private readonly ShellViewModel _parentViewModel;

        public string LogoUrl => _model.LogoUrl;
        public ReadOnlyReactiveProperty<string> ThumbnailUrl { get; private set; }

        public ChannelViewModel(ShellViewModel parentViewModel, Channel channel)
        {
            _parentViewModel = parentViewModel;
            _model = channel;
            ThumbnailUrl = _model.ObserveProperty(x => x.ThumbnailUrl).ToReadOnlyReactiveProperty().AddTo(this);
        }

        // CallMethodAction
        public void ChannelClick()
        {
            _parentViewModel.HostViewModel.Address = $"https://abema.tv/now-on-air/{_model.ChannelType.ToUrlString()}";
        }
    }
}