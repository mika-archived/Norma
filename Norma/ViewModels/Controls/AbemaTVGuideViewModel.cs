using Norma.Eta.Models;
using Norma.Eta.Mvvm;
using Norma.Models;
using Norma.ViewModels.TVGuide;

using Reactive.Bindings;

namespace Norma.ViewModels.Controls
{
    // ReSharper disable once InconsistentNaming
    internal class AbemaTVGuideViewModel : ViewModel
    {
        public ReadOnlyReactiveCollection<ChannelViewModel> Channnels { get; private set; }

        public AbemaTVGuideViewModel(ShellViewModel parent, Configuration c, Models.Timetable t)
        {
            var channels = new AbemaChannels().AddTo(this);
            Channnels = channels.Channels
                                .ToReadOnlyReactiveCollection(w => new ChannelViewModel(parent, new Channel(w, c, t)))
                                .AddTo(this);
        }
    }
}