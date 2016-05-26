using System;

using Norma.Gamma.Models;

namespace Norma.Eta.Models
{
    public class TimetableCache
    {
        public DateTime SyncDateTime { get; set; }

        public Channel[] Channels { get; set; }

        public ChannelSchedule[] ChannelSchedules { get; set; }

        public TimetableCache()
        {
            SyncDateTime = DateTime.MinValue;
        }

        public bool IsSyncNeeded()
        {
            var today = DateTime.Now;
            return
                !(today.Year == SyncDateTime.Year && today.Month == SyncDateTime.Month && today.Day == SyncDateTime.Day);
        }
    }
}