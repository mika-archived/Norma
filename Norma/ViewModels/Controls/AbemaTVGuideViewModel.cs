using Norma.Extensions;
using Norma.Models;
using Norma.ViewModels.Internal;
using Norma.ViewModels.TVGuide;

using Reactive.Bindings;

namespace Norma.ViewModels.Controls
{
    // ReSharper disable once InconsistentNaming
    internal class AbemaTVGuideViewModel : ViewModel
    {
        public ReadOnlyReactiveCollection<ChannelViewModel> Channnels { get; private set; }

        public AbemaTVGuideViewModel(ShellViewModel parentViewModel)
        {
            var channels = new AbemaChannels().AddTo(this);
            Channnels = channels.Channels
                                .ToReadOnlyReactiveCollection(w => new ChannelViewModel(parentViewModel, new Channel(w)))
                                .AddTo(this);
        }
    }
}