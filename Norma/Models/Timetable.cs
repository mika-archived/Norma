using System;
using System.Linq;
using System.Threading.Tasks;

using Norma.Gamma.Models;

namespace Norma.Models
{
    internal class Timetable
    {
        private readonly AbemaApiHost _abemaApiHost;
        public DateTime LastSyncTime { get; private set; }

        public Media Media { get; private set; }

        public Timetable(AbemaApiHost abemaApiHost)
        {
            _abemaApiHost = abemaApiHost;
        }

        public void Sync()
        {
            Media = _abemaApiHost.MediaOfCurrent();
            LastSyncTime = DateTime.Now;
        }

        // ↓名前やばい
        public async Task SyncAsync()
        {
            Media = await _abemaApiHost.MediaOfCurrentAsync();
            LastSyncTime = DateTime.Now;
        }

        public Slot CurrentSlot(AbemaChannel channel)
        {
            var schedule = Media.ChannelSchedules.FirstOrDefault(w => w.ChannelId == channel.ToUrlString());
            return schedule?.Slots.SingleOrDefault(w => w.StartAt <= DateTime.Now && DateTime.Now <= w.EndAt);
        }
    }
}