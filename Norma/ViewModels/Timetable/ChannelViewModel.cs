using System;
using System.Collections.ObjectModel;

using Norma.Gamma.Models;
using Norma.ViewModels.Internal;

using WrapSlot = Norma.Models.Timetables.Slot;

namespace Norma.ViewModels.Timetable
{
    internal class ChannelViewModel : ViewModel
    {
        private readonly Channel _model;
        public string LogoUrl { get; private set; }
        public ObservableCollection<SlotViewModel> Slots { get; }

        public ChannelViewModel(Channel channel, Slot[] slots, DateTime date)
        {
            _model = channel;
            Slots = new ObservableCollection<SlotViewModel>();
            foreach (var slot in slots)
                Slots.Add(new SlotViewModel(new WrapSlot(slot, date)));

            LogoUrl = $"https://hayabusa.io/abema/channels/logo/{channel.Id}.w120.png";
        }
    }
}