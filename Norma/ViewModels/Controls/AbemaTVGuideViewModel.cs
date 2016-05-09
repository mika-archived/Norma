using System;
using System.Collections.ObjectModel;

using Norma.Models;
using Norma.ViewModels.Internal;
using Norma.ViewModels.TVGuide;

namespace Norma.ViewModels.Controls
{
    // ReSharper disable once InconsistentNaming
    internal class AbemaTVGuideViewModel : ViewModel
    {
        public ObservableCollection<ChannelViewModel> ChannnelCollection { get; }

        public AbemaTVGuideViewModel(ShellViewModel parentViewModel)
        {
            ChannnelCollection = new ObservableCollection<ChannelViewModel>();
            foreach (var value in Enum.GetValues(typeof(AbemaChannel)))
                ChannnelCollection.Add(new ChannelViewModel(parentViewModel, new Channel((AbemaChannel) value)));
        }
    }
}