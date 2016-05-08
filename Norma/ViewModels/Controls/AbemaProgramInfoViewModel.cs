using Norma.Helpers;
using Norma.Models;
using Norma.ViewModels.Internal;

using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace Norma.ViewModels.Controls
{
    internal class AbemaProgramInfoViewModel : ViewModel
    {
        private readonly ProgramHost _programHost;

        public ReactiveProperty<string> Title { get; private set; }
        public ReactiveProperty<string> Description { get; private set; }
        public ReactiveProperty<bool> HasInfo { get; private set; }

        public AbemaProgramInfoViewModel(AbemaHostViewModel hostViewModel)
        {
            _programHost = new ProgramHost();
            Title = _programHost.ObserveProperty(w => w.Title).ToReactiveProperty();
            Description = _programHost.ObserveProperty(w => w.Description).ToReactiveProperty();
            HasInfo = _programHost.ObserveProperty(w => w.HasInfo).ToReactiveProperty();
            hostViewModel.Subscribe(nameof(hostViewModel.Address), w =>
            {
                if (!hostViewModel.Address.StartsWith("https://abema.tv/now-on-air/"))
                    return;
                _programHost.OnChannelChanged(AbemaChannelExt.FromUrlString(hostViewModel.Address));
            });
        }
    }
}