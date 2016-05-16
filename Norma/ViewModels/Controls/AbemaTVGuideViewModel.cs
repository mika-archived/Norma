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

        public AbemaTVGuideViewModel(ShellViewModel parent, Configuration c)
        {
            var channels = new AbemaChannels().AddTo(this);
            Channnels = channels.Channels
                                .ToReadOnlyReactiveCollection(w => new ChannelViewModel(parent, new Channel(w, c)))
                                .AddTo(this);
        }
    }
}