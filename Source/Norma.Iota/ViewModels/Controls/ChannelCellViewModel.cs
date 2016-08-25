using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Norma.Delta.Models;
using Norma.Eta.Mvvm;
using Norma.Iota.Models;

namespace Norma.Iota.ViewModels.Controls
{
    internal class ChannelCellViewModel : ViewModel
    {
        public string LogoUrl { get; private set; }
        public ObservableCollection<EpisodeCellViewModel> Slots { get; }

        public ChannelCellViewModel(DateTime date, Channel channel, IEnumerable<Slot> slots)
        {
            Slots = new ObservableCollection<EpisodeCellViewModel>();
            foreach (var slot in slots)
            {
                var pcvm = new EpisodeCellViewModel(new WrapSlot(slot, date)).AddTo(this);
                Slots.Add(pcvm);
            }

            LogoUrl = $"https://hayabusa.io/abema/channels/logo/{channel.ChannelId}.w120.png";
        }

        #region Overrides of ViewModel

        public override void Dispose()
        {
            base.Dispose();
            Slots.Clear();
        }

        #endregion
    }
}