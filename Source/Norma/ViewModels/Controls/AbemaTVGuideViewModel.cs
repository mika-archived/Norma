using System;

using Norma.Delta.Models;
using Norma.Eta.Models;
using Norma.Eta.Mvvm;
using Norma.Eta.Services;
using Norma.Models;
using Norma.ViewModels.TVGuide;

using Reactive.Bindings;

namespace Norma.ViewModels.Controls
{
    // ReSharper disable once InconsistentNaming
    internal class AbemaTVGuideViewModel : ViewModel
    {
        private readonly AbemaState _abemaState;
        private readonly Configuration _configuration;
        public ReadOnlyReactiveCollection<ChannelViewModel> Channnels { get; private set; }

        private Func<Channel, ChannelViewModel> Func => w => new ChannelViewModel(_abemaState, new AbemaChannel(w), _configuration);

        public AbemaTVGuideViewModel(AbemaState abemaState, Configuration configuration, TimetableService timetableService)
        {
            _abemaState = abemaState;
            _configuration = configuration;

            if (!_configuration.Root.Operation.IsShowFavoriteOnly)
                Channnels = timetableService.CurrentSlots
                                            .ToReadOnlyReactiveCollection(w => Func(w.Channel))
                                            .AddTo(this);
            else
                Channnels = timetableService.CurrentFavSlots
                                            .ToReadOnlyReactiveCollection(w => Func(w.Channel))
                                            .AddTo(this);
        }
    }
}