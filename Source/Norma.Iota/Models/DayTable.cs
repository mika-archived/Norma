using System;
using System.Collections.ObjectModel;
using System.Linq;

using Norma.Delta.Models;
using Norma.Delta.Services;

namespace Norma.Iota.Models
{
    internal class DayTable
    {
        private readonly DatabaseService _databaseService;
        public ObservableCollection<ChannelTable> ChannelTable { get; }
        public ObservableCollection<DateTime> AvailableDates { get; }

        public DayTable(DatabaseService databaseService)
        {
            _databaseService = databaseService;
            ChannelTable = new ObservableCollection<ChannelTable>();
            AvailableDates = new ObservableCollection<DateTime>();
            InQuery();
        }

        private void InQuery()
        {
            using (var connection = _databaseService.Connect())
            {
                var lasySyncTime = DateTime.Parse(connection.Metadata.Single(w => w.Key == Metadata.LastSyncTimeKey).Value);
                for (var i = 0; i < 6; i++)
                    AvailableDates.Add(lasySyncTime.AddDays(i));
            }
        }

        public void Query(DateTime dateTime)
        {
            ChannelTable.Clear();
            var maxDate = dateTime.AddHours(23).AddMinutes(59).AddSeconds(59); // 23:59:59
            using (var connection = _databaseService.Connect())
            {
                connection.TurnOffLazyLoading();
                var channels = connection.Channels.AsNoTracking().OrderBy(w => w.Order).ToList();
                foreach (var channel in channels)
                {
                    var slots = connection.Slots.AsNoTracking().Where(w => w.Channel.ChannelId == channel.ChannelId)
                                          .Where(w => dateTime <= w.EndAt && w.StartAt <= maxDate).ToList();
                    ChannelTable.Add(new ChannelTable(dateTime, channel, slots));
                }
            }
        }
    }
}