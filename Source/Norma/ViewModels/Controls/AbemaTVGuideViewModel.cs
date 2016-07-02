using System;

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
        private readonly Configuration _configuration;
        private readonly ShellViewModel _parent;
        private readonly Timetable _timetable;
        public ReadOnlyReactiveCollection<ChannelViewModel> Channnels { get; private set; }
        private Func<AbemaChannel, Channel> Func => v => new Channel(v, _configuration, _timetable);

        public AbemaTVGuideViewModel(ShellViewModel parent, Configuration configuration, Timetable timetable)
        {
            _parent = parent;
            _configuration = configuration;
            _timetable = timetable;

            var channels = new AbemaChannels(_timetable);
            Channnels = channels.Channels
                                .ToReadOnlyReactiveCollection(v => new ChannelViewModel(_parent, Func(v)))
                                .AddTo(this);
        }
    }
}