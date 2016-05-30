using System;
using System.Collections.ObjectModel;

using Norma.Eta.Mvvm;
using Norma.Gamma.Models;
using Norma.Iota.Models;

namespace Norma.Iota.ViewModels.Controls
{
    internal class ChannelCellViewModel : ViewModel
    {
        public string LogoUrl { get; private set; }
        public ObservableCollection<ProgramCellViewModel> Slots { get; }

        public ChannelCellViewModel(Channel channel, Slot[] slots, DateTime date)
        {
            Slots = new ObservableCollection<ProgramCellViewModel>();
            foreach (var slot in slots)
            {
                var pcvm = new ProgramCellViewModel(new WrapSlot(slot, date)).AddTo(this);
                Slots.Add(pcvm);
            }

            LogoUrl = $"https://hayabusa.io/abema/channels/logo/{channel.Id}.w120.png";
        }
    }
}