using System.Collections.ObjectModel;

using Norma.Eta.Models;

using Prism.Mvvm;

using Reactive.Bindings;

namespace Norma.Models
{
    internal class AbemaChannels : BindableBase
    {
        public ReadOnlyObservableCollection<AbemaChannel> Channels { get; }

        public AbemaChannels(Timetable timetable)
        {
            Channels = timetable.CurrentChannels.ToReadOnlyReactiveCollection(AbemaChannelExt.FromUrlString);
        }
    }
}