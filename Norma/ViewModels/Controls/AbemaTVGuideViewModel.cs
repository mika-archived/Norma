using System;
using System.Collections.ObjectModel;

using Norma.Models;
using Norma.ViewModels.Internal;

namespace Norma.ViewModels.Controls
{
    // ReSharper disable once InconsistentNaming
    internal class AbemaTVGuideViewModel : ViewModel
    {
        public ObservableCollection<Channel> ChannnelCollection { get; }

        public AbemaTVGuideViewModel()
        {
            ChannnelCollection = new ObservableCollection<Channel>();
            foreach (var value in Enum.GetValues(typeof(AbemaChannels)))
                ChannnelCollection.Add(new Channel((AbemaChannels) value));
        }
    }
}