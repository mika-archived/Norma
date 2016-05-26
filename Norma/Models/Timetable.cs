using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Norma.Eta;
using Norma.Eta.Models;
using Norma.Gamma.Models;
using Norma.Models.Timetables;

using Slot = Norma.Gamma.Models.Slot;

namespace Norma.Models
{
    internal class Timetable
    {
        private readonly AbemaApiHost _abemaApiHost;
        private TimetableCache _cache;

        public Gamma.Models.Channel[] Channels
        {
            get { return _cache.Channels; }
            set { _cache.Channels = value; }
        }

        public ChannelSchedule[] ChannelSchedules
        {
            get { return _cache.ChannelSchedules; }
            set { _cache.ChannelSchedules = value; }
        }

        public DateTime LastSyncTime => _cache.SyncDateTime;

        public Timetable(AbemaApiHost abemaApiHost)
        {
            _abemaApiHost = abemaApiHost;
            _cache = new TimetableCache();
            Load();
        }

        public void Sync()
        {
            if (!_cache.IsSyncNeeded())
                return;
            var media = _abemaApiHost.MediaOfOneWeek();
            _cache.SyncDateTime = DateTime.Now;
            Channels = media.Channels;
            ChannelSchedules = media.ChannelSchedules;
        }

        // ↓名前やばい
        public async Task SyncAsync()
        {
            if (!_cache.IsSyncNeeded())
                return;
            var media = await _abemaApiHost.MediaOfCurrentAsync();
            _cache.SyncDateTime = DateTime.Now;
            Channels = media.Channels;
            ChannelSchedules = media.ChannelSchedules;
        }

        private void Load()
        {
            if (!File.Exists(NormaConstants.TimetableCacheFile))
                return;
            using (var sr = File.OpenText(NormaConstants.TimetableCacheFile))
            {
                var serializer = new JsonSerializer();
                _cache = (TimetableCache) serializer.Deserialize(sr, typeof(TimetableCache));
            }
        }

        public void Save()
        {
            using (var sw = File.CreateText(NormaConstants.TimetableCacheFile))
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(sw, _cache);
            }
        }

        public Slot CurrentSlot(AbemaChannel channel)
        {
            var schedule = ChannelSchedules.FirstOrDefault(w => w.ChannelId == channel.ToUrlString());
            return schedule?.Slots.SingleOrDefault(w => w.StartAt <= DateTime.Now && DateTime.Now <= w.EndAt);
        }
    }
}