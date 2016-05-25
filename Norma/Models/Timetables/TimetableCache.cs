using System;

using Norma.Gamma.Models;

using InternalChannel = Norma.Gamma.Models.Channel;

namespace Norma.Models.Timetables
{
    internal class TimetableCache
    {
        public DateTime SyncDateTime { get; set; }

        public InternalChannel[] Channels { get; set; }

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