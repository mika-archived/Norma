using System;
using System.Collections.ObjectModel;

using Norma.Eta.Mvvm;
using Norma.Gamma.Models;

using WrapSlot = Norma.Models.Timetables.Slot;

namespace Norma.ViewModels.Timetable
{
    internal class ChannelViewModel : ViewModel
    {
        public string LogoUrl { get; private set; }
        public ObservableCollection<SlotViewModel> Slots { get; }

        public ChannelViewModel(Channel channel, Slot[] slots, DateTime date)
        {
            Slots = new ObservableCollection<SlotViewModel>();
            foreach (var slot in slots)
            {
                var vm = new SlotViewModel(new WrapSlot(slot, date)).AddTo(this);
                Slots.Add(vm);
            }

            LogoUrl = $"https://hayabusa.io/abema/channels/logo/{channel.Id}.w120.png";
        }
    }
}