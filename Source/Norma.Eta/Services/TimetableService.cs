using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

using Norma.Delta.Models;
using Norma.Delta.Services;
using Norma.Eta.Extensions;
using Norma.Eta.Filters;
using Norma.Eta.Models;
using Norma.Gamma.Models;

using static Norma.Eta.Helpers.DateTimeHelper;
using static Norma.Eta.Helpers.ApiModelToFlatModelHelper;

using Episode = Norma.Delta.Models.Episode;
using Series = Norma.Delta.Models.Series;
using Slot = Norma.Delta.Models.Slot;

namespace Norma.Eta.Services
{
    public class TimetableService
    {
        private readonly AbemaApiClient _abemaApiClient;
        private readonly DatabaseService _databaseService;

        private readonly List<IFilter> _filters = new List<IFilter>
        {
            new MBToSBFilter(),
            new CopyrightFilter(),
            new RoleFilter(),
            new InvalidBracesFilter(),
            new BracesFilter(),
            new SpaceFilter(),
            new EmptyFilter(),
            new SeparatorFilter()
        };

        public TimetableService(AbemaApiClient abemaApiClient, DatabaseService databaseService)
        {
            _abemaApiClient = abemaApiClient;
            _databaseService = databaseService;
            _currentSlotInternal = new ObservableCollection<Slot>();
        }

        public void Initialize()
        {
            using (var connection = _databaseService.Connect())
            {
                var lastSyncTimeStr = connection.Metadata.Single(w => w.Key == Metadata.LastSyncTimeKey);
                lastSyncTimeStr.Value = DateTime.Now.ToString("G");
                connection.DetectChanges();
                connection.SaveChanges();

                if (!EqualsWithDates(DateTime.Today, DateTime.Parse(lastSyncTimeStr.Value)))
                {
                    var timetable = _abemaApiClient.MediaOfDays(6);

                    // チャンネル ~ 200ms
                    UpdateChannels(connection, timetable);

                    // クレジット ~ 900ms
                    UpdateCredits(connection, timetable.ChannelSchedules);

                    // シリーズ ~ 200ms
                    UpdateSeries(connection, timetable.ChannelSchedules);

                    // 番組単位 ~ 3000ms (ﾂﾗｲ)
                    UpdateEpisodes(connection, timetable.ChannelSchedules);

                    // 放送単位 ~ 1000ms
                    UpdateSlots(connection, timetable.ChannelSchedules);
                }
            }
            Observable.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(1)).Subscribe(async w => await UpdateAsync());
        }

        private async Task UpdateAsync()
        {
            List<Slot> slots;
            using (var connection = _databaseService.Connect())
            {
                var lastSyncTime = connection.Metadata.AsNoTracking().Single(w => w.Key == Metadata.LastSyncTimeKey);
                if (!EqualsWithDates(DateTime.Today, DateTime.Parse(lastSyncTime.Value)))
                    // TODO: 更新処理
                    return;
                var datetime = DateTime.Now;
                slots = connection.Slots.AsNoTracking()
                                  .Where(w => w.StartAt <= datetime)
                                  .Where(w => datetime <= w.EndAt)
                                  .Include(w => w.Channel)
                                  .OrderBy(w => w.Channel.Order)
                                  .ToList();
            }

            // 削除
            foreach (var slot in _currentSlotInternal.ToArray())
            {
                if (slots.Any(w => w.SlotId == slot.SlotId))
                    continue;
                _currentSlotInternal.Remove(slot);
            }
            // 追加
            foreach (var items in slots.Select((w, i) => new {Slot = w, Index = i}))
            {
                if (_currentSlotInternal.Any(w => w.SlotId == items.Slot.SlotId))
                    continue;
                _currentSlotInternal.Insert(items.Index, items.Slot);
            }
        }

        #region CurrentSlots

        private readonly ObservableCollection<Slot> _currentSlotInternal;

        private ReadOnlyObservableCollection<Slot> _currentSlots;

        public ReadOnlyObservableCollection<Slot> CurrentSlots
            => _currentSlots ?? (_currentSlots = new ReadOnlyObservableCollection<Slot>(_currentSlotInternal));

        #endregion

        #region JSON data to SQL flatten data

        private void UpdateChannels(DbConnection connection, Media timetable)
        {
            foreach (var channel in timetable.Channels)
                connection.Channels.AddIfNotExists(channel.ConvertToChannel(), w => w.ChannelId == channel.Id);
            connection.SaveChanges();
        }

        private void UpdateCredits(DbConnection connection, IEnumerable<ChannelSchedule> schedules)
        {
            // ここは ~ 200ms で終わるのに
            var programs = schedules.SelectMany(w => w.Slots).SelectMany(w => w.Programs).Select(w => w.Credit).ToList();
            var casts = programs.Where(w => w.Cast != null).SelectMany(w => w.Cast).Select(Filter).Distinct().ToList();
            var crews = programs.Where(w => w.Crews != null).SelectMany(w => w.Crews).Select(Filter).Distinct().ToList();
            var copyrights = programs.Where(w => w.Copyrights != null)
                                     .SelectMany(w => w.Copyrights)
                                     .Select(Filter)
                                     .Distinct()
                                     .ToList();

            // 先に SELECT しておくことで、 casts 回 EXISTS が走るのを防ぐ
            var castList = connection.Casts.ToList();
            casts.ForEach(w => castList.AddIfNotExists(connection.Casts, new Cast {Name = w}, v => v.Name == w));

            var crewList = connection.Crews.ToList();
            crews.ForEach(w => crewList.AddIfNotExists(connection.Crews, new Crew {Name = w}, v => v.Name == w));

            var copyrightList = connection.Copyrights.ToList();
            copyrights.ForEach(w => copyrightList.AddIfNotExists(connection.Copyrights, new Copyright {Name = w}, v => v.Name == w));
            connection.SaveChanges();
        }

        private void UpdateSeries(DbConnection connection, ChannelSchedule[] schedules)
        {
            var seriesIds = schedules.SelectMany(w => w.Slots).SelectMany(w => w.Programs).Select(w => w.Series.Id).Distinct().ToList();

            // 先に SELECT
            var list = connection.Series.ToList();
            seriesIds.ForEach(w => list.AddIfNotExists(connection.Series, new Series {SeriesId = w}, v => v.SeriesId == w));
            connection.SaveChanges();
        }

        private void UpdateEpisodes(DbConnection connection, ChannelSchedule[] schedules)
        {
            var rawEpisodes = schedules.SelectMany(w => w.Slots).SelectMany(w => w.Programs).Distinct().ToList();

            var episodes = new List<Episode>();
            // 先に SELECT しておく
            var episodeList = connection.Episodes.ToList();
            var castList = connection.Casts.ToList();
            var copyRightList = connection.Copyrights.ToList();
            var crewList = connection.Crews.ToList();
            var seriesList = connection.Series.ToList();

            foreach (var rawEpisode in rawEpisodes)
            {
                var episode = rawEpisode.ConvertToEpisode();
                if (rawEpisode.Credit.Cast != null)
                    foreach (var cast in rawEpisode.Credit.Cast.Select(Filter))
                        episode.Casts.Add(castList.Single(w => w.Name == cast));
                if (rawEpisode.Credit.Copyrights != null)
                    foreach (var copyright in rawEpisode.Credit.Copyrights.Select(Filter))
                        episode.Copyrights.Add(copyRightList.Single(w => w.Name == copyright));
                if (rawEpisode.Credit.Crews != null)
                    foreach (var crew in rawEpisode.Credit.Crews.Select(Filter))
                        episode.Crews.Add(crewList.Single(w => w.Name == crew));
                foreach (var thumbnail in rawEpisode.ProvidedInfo.ConvertToThumbnail())
                    episode.Thumbnails.Add(thumbnail);
                episode.Series = seriesList.SingleOrDefault(w => w.SeriesId == rawEpisode.Series.Id);
                episodes.Add(episode);
            }
            episodes.ForEach(w => episodeList.AddIfNotExists(connection.Episodes, w, v => v.EpisodeId == w.EpisodeId));
            connection.SaveChanges();
        }

        private void UpdateSlots(DbConnection connection, ChannelSchedule[] schedules)
        {
            var slots = new List<Slot>();
            var channelList = connection.Channels.ToList();
            var episodeList = connection.Episodes.ToList();
            var slotList = connection.Slots.ToList();

            foreach (var schedule in schedules)
            {
                var channel = channelList.Single(w => w.ChannelId == schedule.ChannelId);
                foreach (var rawSlot in schedule.Slots)
                {
                    var slot = rawSlot.ConvertToSlot();
                    slot.Channel = channel;
                    foreach (var program in rawSlot.Programs)
                        slot.Episodes.Add(episodeList.Single(w => w.EpisodeId == program.Id));
                    slots.Add(slot);
                }
            }
            slots.ForEach(w => slotList.AddIfNotExists(connection.Slots, w, v => v.SlotId == w.SlotId));
            connection.SaveChanges();
        }

        private string Filter(string str) => _filters.Aggregate(str, (current, filter) => filter.Call(current));

        #endregion
    }
}