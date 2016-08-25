using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Norma.Delta.Models;

namespace Norma.Iota.Models
{
    internal class ChannelTable
    {
        public DateTime Date { get; }
        public Channel Channel { get; }

        public ReadOnlyCollection<Slot> Slots { get; }

        public ChannelTable(DateTime date, Channel channel, List<Slot> slots)
        {
            Date = date;
            Channel = channel;
            Slots = slots.AsReadOnly();
        }
    }
}