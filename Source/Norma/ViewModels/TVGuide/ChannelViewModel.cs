using System.Reactive.Linq;

using Norma.Eta.Mvvm;
using Norma.Models;

using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace Norma.ViewModels.TVGuide
{
    internal class ChannelViewModel : ViewModel
    {
        private readonly AbemaState _abemaState;
        private readonly AbemaChannel _model;

        public string LogoUrl => _model.LogoUrl;
        public ReadOnlyReactiveProperty<string> Title { get; private set; }
        public ReadOnlyReactiveProperty<string> StartTime { get; private set; }
        public ReadOnlyReactiveProperty<string> EndTime { get; private set; }
        public ReadOnlyReactiveProperty<string> ThumbnailUrl { get; private set; }

        public ChannelViewModel(AbemaState abemaState, AbemaChannel channel)
        {
            _abemaState = abemaState;
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
        public void ChannelClick() => _abemaState.CurrentChannel = _model.Channel;
    }
}