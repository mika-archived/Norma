using System;

using Norma.Delta.Models;
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
        private readonly AbemaState _abemaState;
        public ReadOnlyReactiveCollection<ChannelViewModel> Channnels { get; private set; }

        private Func<Channel, ChannelViewModel> Func => w => new ChannelViewModel(_abemaState, new AbemaChannel(w));

        public AbemaTVGuideViewModel(AbemaState abemaState, TimetableService timetableService)
        {
            _abemaState = abemaState;
            Channnels = timetableService.CurrentSlots
                                        .ToReadOnlyReactiveCollection(w => Func(w.Channel))
                                        .AddTo(this);
        }
    }
}