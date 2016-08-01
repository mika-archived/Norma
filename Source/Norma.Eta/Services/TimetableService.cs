using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

using Norma.Delta.Models;
using Norma.Delta.Services;
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
        private readonly ObservableCollection<Slot> _currentSlotsInternal;
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
            _currentSlotsInternal = new ObservableCollection<Slot>();
        }

        public void Initialize()
        {
            using (var dbConnection = _databaseService.Connect())
            {
                var lastSyncTimeStr = dbConnection.Metadata.SingleOrDefault(w => w.Key == Metadata.LastSyncTimeKey);
                var lastSyncTime = DateTime.MinValue;
                if (!string.IsNullOrWhiteSpace(lastSyncTimeStr?.Value))
                    lastSyncTime = DateTime.Parse(lastSyncTimeStr.Value);

                if (EqualsWithDates(DateTime.Today, lastSyncTime))
                    return;

                // 1日分
                var timetable = _abemaApiClient.MediaOfDays(6);
                Debug.WriteLine(timetable);

                // チャンネル ~ 200ms
                UpdateChannels(dbConnection, timetable);

                // クレジット ~ 900ms
                UpdateCredits(dbConnection, timetable.ChannelSchedules);

                // シリーズ ~ 200ms
                UpdateSeries(dbConnection, timetable.ChannelSchedules);

                // 番組単位 ~ 3000ms (ﾂﾗｲ)
                UpdateEpisodes(dbConnection, timetable.ChannelSchedules);

                // 放送単位
                UpdateSlots(timetable.ChannelSchedules);
            }

            Observable.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(1)).Subscribe(async w => await UpdateAsync());
        }

        private async Task UpdateAsync()
        {

        }

        #region JSON data to SQL flatten data

        private void UpdateChannels(DbConnection dbConnection, Media timetable)
        {
            foreach (var channel in timetable.Channels)
                dbConnection.Channels.AddIfNotExists(channel.ConvertToChannel(), w => w.ChannelId == channel.Id);
            dbConnection.SaveChanges();
        }

        private void UpdateCredits(DbConnection dbConnection, IEnumerable<ChannelSchedule> schedules)
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
            var castList = dbConnection.Casts.ToList();
            casts.ForEach(w => castList.AddIfNotExists(dbConnection.Casts, new Cast {Name = w}, v => v.Name == w));

            var crewList = dbConnection.Crews.ToList();
            crews.ForEach(w => crewList.AddIfNotExists(dbConnection.Crews, new Crew {Name = w}, v => v.Name == w));

            var copyrightList = dbConnection.Copyrights.ToList();
            copyrights.ForEach(w => copyrightList.AddIfNotExists(dbConnection.Copyrights, new Copyright {Name = w}, v => v.Name == w));
            dbConnection.SaveChanges();
        }

        private void UpdateSeries(DbConnection dbConnection, ChannelSchedule[] schedules)
        {
            var seriesIds = schedules.SelectMany(w => w.Slots).SelectMany(w => w.Programs).Select(w => w.Series.Id).Distinct().ToList();

            // 先に SELECT
            var list = dbConnection.Series.ToList();
            seriesIds.ForEach(w => list.AddIfNotExists(dbConnection.Series, new Series {SeriesId = w}, v => v.SeriesId == w));
            dbConnection.SaveChanges();
        }

        private void UpdateEpisodes(DbConnection dbConnection, ChannelSchedule[] schedules)
        {
            var rawEpisodes = schedules.SelectMany(w => w.Slots).SelectMany(w => w.Programs).Distinct().ToList();

            var episodes = new List<Episode>();
            // 先に SELECT しておく
            var episodeList = dbConnection.Episodes.ToList();
            var castList = dbConnection.Casts.ToList();
            var copyRightList = dbConnection.Copyrights.ToList();
            var crewList = dbConnection.Crews.ToList();
            var seriesList = dbConnection.Series.ToList();

            foreach (var rawEpisode in rawEpisodes)
            {
                var episode = rawEpisode.ConvertToEpisode();

                // キャスト
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
            episodes = episodes.ToList();

            episodes.ForEach(w => episodeList.AddIfNotExists(dbConnection.Episodes, w, v => v.EpisodeId == w.EpisodeId));
            dbConnection.SaveChanges();
        }

        private void UpdateSlots(ChannelSchedule[] schedules)
        {

        }

        private string Filter(string str) => _filters.Aggregate(str, (current, filter) => filter.Call(current));

        #endregion

        #region CurrentSlots

        private ReadOnlyObservableCollection<Slot> _currentSlots;

        public ReadOnlyObservableCollection<Slot> CurrentSlots
            => _currentSlots ?? (_currentSlots = new ReadOnlyObservableCollection<Slot>(_currentSlotsInternal));

        #endregion
    }

    internal static class DbExt
    {
        public static void AddIfNotExists<T>(this DbSet<T> obj, T item, Func<T, bool> condition)
            where T : class
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            if (obj.Any(condition.Invoke))
                return;
            obj.Add(item);
        }

        public static void AddIfNotExists<T>(this IEnumerable<T> obj, DbSet<T> db, T item, Func<T, bool> condition)
            where T : class
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            if (obj.Any(condition.Invoke))
                return;
            db.Add(item);
        }
    }
}