using System.Reactive.Linq;

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
        public ReadOnlyReactiveProperty<string> Title { get; private set; }
        public ReadOnlyReactiveProperty<string> StartTime { get; private set; }
        public ReadOnlyReactiveProperty<string> EndTime { get; private set; }
        public ReadOnlyReactiveProperty<string> ThumbnailUrl { get; private set; }

        public ChannelViewModel(ShellViewModel parentViewModel, Channel channel)
        {
            _parentViewModel = parentViewModel;
            _model = channel;
            Title = _model.ObserveProperty(w => w.Title)
                          .ToReadOnlyReactiveProperty()
                          .AddTo(this);
            StartTime = _model.ObserveProperty(w => w.StartAt)
                              .Select(w => w.ToString("HH:mm"))
                              .ToReadOnlyReactiveProperty()
                              .AddTo(this);
            EndTime = _model.ObserveProperty(w => w.EndAt)
                            .Select(w => w.ToString("HH:mm"))
                            .ToReadOnlyReactiveProperty()
                            .AddTo(this);
            ThumbnailUrl = _model.ObserveProperty(x => x.ThumbnailUrl).ToReadOnlyReactiveProperty().AddTo(this);
        }

        // CallMethodAction
        public void ChannelClick()
        {
            _parentViewModel.HostViewModel.Address = $"https://abema.tv/now-on-air/{_model.ChannelType.ToUrlString()}";
        }
    }
}