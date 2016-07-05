using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Norma.Eta.Extensions;
using Norma.Gamma.Models;

using static Norma.Eta.Models.DateTimeHelper;

namespace Norma.Eta.Models
{
    public class Timetable
    {
        private readonly AbemaApiHost _abemaApiHost;
        private TimetableCache _cache;

        public Channel[] Channels
        {
            get { return _cache.Channels; }
            set { _cache.Channels = value; }
        }

        public ChannelSchedule[] ChannelSchedules
        {
            get { return _cache.ChannelSchedules; }
            set { _cache.ChannelSchedules = value; }
        }

        public ObservableCollection<string> CurrentChannels { get; }

        public DateTime LastSyncTime => _cache.SyncDateTime;

        public Timetable(AbemaApiHost abemaApiHost)
        {
            _abemaApiHost = abemaApiHost;
            _cache = new TimetableCache();
            CurrentChannels = new ObservableCollection<string>();
            Load();
            Observable.Timer(TimeSpan.Zero, TimeSpanExt.OneSecond).Subscribe(async w => await UpdateCurrentChannels());
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
        private async Task SyncAsync()
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

        private async Task UpdateCurrentChannels()
        {
            try
            {
                await SyncAsync();
                var channelSlots = ChannelSchedules.Where(w => EqualsWithDates(w.Date, DateTime.Today))
                                                   .Select(w => new {w.Slots, w.ChannelId});

                var channels = new List<string>();
                foreach (var channelSlot in channelSlots)
                {
                    if (!channelSlot.Slots.Any(w => IsRangeOf(w.StartAt, w.EndAt, DateTime.Now)))
                        continue;
                    channels.Add(channelSlot.ChannelId);
                }

                var temp = new List<string>(CurrentChannels);
                foreach (var channel in temp)
                {
                    if (!channels.Contains(channel))
                        CurrentChannels.Remove(channel);
                }
                foreach (var channel in channels.Select((v, i) => new {Value = v, Index = i}))
                {
                    if (CurrentChannels.Contains(channel.Value))
                        continue;
                    CurrentChannels.Insert(channel.Index, channel.Value);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }
    }
}