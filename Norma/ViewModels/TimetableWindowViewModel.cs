using System;
using System.Collections.ObjectModel;
using System.Linq;

using Norma.ViewModels.Internal;
using Norma.ViewModels.Timetable;

using ModelTimetable = Norma.Models.Timetable;

namespace Norma.ViewModels
{
    internal class TimetableWindowViewModel : ViewModel
    {
        private readonly int _index; // 日付管理用(0 = 今日, 6 = 一週間後みたいな)
        public ObservableCollection<ChannelViewModel> Channels { get; }

        public TimetableWindowViewModel(ModelTimetable timetable)
        {
            _index = (DateTime.Now - timetable.LastSyncTime).Days;
            Channels = new ObservableCollection<ChannelViewModel>();
            foreach (var channel in timetable.Channels)
            {
                var slots = timetable.ChannelSchedules.Where(w => w.ChannelId == channel.Id).ElementAt(_index);
                Channels.Add(new ChannelViewModel(channel, slots.Slots, slots.Date));
            }
        }
    }
}