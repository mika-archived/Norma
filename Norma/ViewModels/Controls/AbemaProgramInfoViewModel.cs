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

        public ReadOnlyReactiveProperty<string> Title { get; private set; }
        public ReadOnlyReactiveProperty<string> Description { get; private set; }
        public ReadOnlyReactiveProperty<bool> HasInfo { get; private set; }
        public ReadOnlyReactiveProperty<string> Thumbnail1 { get; private set; }
        public ReadOnlyReactiveProperty<string> Thumbnail2 { get; private set; }

        public AbemaProgramInfoViewModel(AbemaHostViewModel hostViewModel)
        {
            _programHost = new ProgramHost();
            Title = _programHost.ObserveProperty(w => w.Title).ToReadOnlyReactiveProperty();
            Description = _programHost.ObserveProperty(w => w.Description).ToReadOnlyReactiveProperty();
            HasInfo = _programHost.ObserveProperty(w => w.HasInfo).ToReadOnlyReactiveProperty();
            Thumbnail1 = _programHost.ObserveProperty(w => w.Thumbnail1).ToReadOnlyReactiveProperty();
            Thumbnail2 = _programHost.ObserveProperty(w => w.Thumbnail2).ToReadOnlyReactiveProperty();
            hostViewModel.Subscribe(nameof(hostViewModel.Address), w =>
            {
                if (!hostViewModel.Address.StartsWith("https://abema.tv/now-on-air/"))
                    return;
                _programHost.OnChannelChanged(AbemaChannelExt.FromUrlString(hostViewModel.Address));
            });
        }
    }
}