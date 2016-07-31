using Microsoft.Practices.ServiceLocation;

using Norma.Delta.Services;
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

        public AbemaTVGuideViewModel()
        {
            var timetableService = ServiceLocator.Current.GetInstance<TimetableService>();
            Channnels = timetableService.CurrentSlots
                                        .ToReadOnlyReactiveCollection(
                                                                      v =>
                                                                          new ChannelViewModel(
                                                                          new AbemaChannel(v.Channel.Id)))
                                        .AddTo(this);
        }
    }
}