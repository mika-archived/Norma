using System;
using System.Data.Entity.Infrastructure;
using System.Linq;

using Norma.Delta.Models;
using Norma.Delta.Models.Enums;

namespace Norma.Delta.Services
{
    /// <summary>
    ///     予約の作成、参照、更新、削除を行います。
    /// </summary>
    public class ReservationService
    {
        private readonly DatabaseService _databaseService;

        public ReservationService(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        #region Time

        public DbQuery<TimeReservation> TimeReservations => _databaseService.TimeReservations.AsNoTracking();

        public void InsertTimeReservaion(DateTime startAt, Repetition repetition, DateRange range = null)
        {
            if (range == null)
                range = DateRange.Unspecified;

            _databaseService.TimeReservations.Add(new TimeReservation
            {
                Reservation = new Reservation
                {
                    IsEnabled = true,
                    Range = range
                },
                StartAt = startAt,
                Repetition = repetition
            });
            _databaseService.SaveChanges();
        }

        public void UpdateTimeReservation(TimeReservation timeReservation)
        {
            var rsv = _databaseService.Reservations.SingleOrDefault(w => w.Id == timeReservation.Reservation.Id);
            if (rsv == null)
                throw new InvalidOperationException();
            rsv.TimeReservation.Merge(timeReservation);
            _databaseService.SaveChanges();
        }

        public void DeleteTimeReservation(TimeReservation timeReservation)
        {
            timeReservation.Reservation.IsEnabled = false;
            _databaseService.SaveChanges();
        }

        #endregion

        #region Keyword

        public DbQuery<KeywordReservation> KeywordReservations => _databaseService.KeywordReservations.AsNoTracking();

        public void InsertKeywordReservation(string keyword, bool isRegex, DateRange range = null)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                throw new ArgumentException(nameof(keyword));

            if (range == null)
                range = DateRange.Unspecified;

            _databaseService.KeywordReservations.Add(new KeywordReservation
            {
                Reservation = new Reservation
                {
                    IsEnabled = true,
                    Range = range
                },
                Keyword = keyword,
                IsRegex = isRegex
            });
            _databaseService.SaveChanges();
        }

        public void UpdateKeywordReservation(KeywordReservation keywordReservation)
        {
            var rsv = _databaseService.Reservations.SingleOrDefault(w => w.Id == keywordReservation.Reservation.Id);
            if (rsv == null)
                throw new InvalidOperationException();
            rsv.KeywordReservation.Merge(keywordReservation);
            _databaseService.SaveChanges();
        }

        public void DeleteKeywordReservation(KeywordReservation keywordReservation)
        {
            keywordReservation.Reservation.IsEnabled = false;
            _databaseService.SaveChanges();
        }

        #endregion

        #region Series

        public DbQuery<SeriesReservation> SeriesReservations => _databaseService.SeriesReservations.AsNoTracking();

        public void InsertSeriesReservation(Series series, DateRange range = null)
        {
            if (series == null)
                throw new ArgumentException(nameof(series));

            if (range == null)
                range = DateRange.Unspecified;

            _databaseService.SeriesReservations.Add(new SeriesReservation
            {
                Reservation = new Reservation
                {
                    IsEnabled = true,
                    Range = range
                },
                Series = series
            });

            _databaseService.SaveChanges();
        }

        public void UpdateSeriesReservation(SeriesReservation seriesReservation)
        {
            var rsv = _databaseService.Reservations.SingleOrDefault(w => w.Id == seriesReservation.Reservation.Id);
            if (rsv == null)
                throw new InvalidOperationException();

            rsv.SeriesReservation.Merge(seriesReservation);
            _databaseService.SaveChanges();
        }

        public void DeleteSeriesReservation(SeriesReservation seriesReservation)
        {
            seriesReservation.Reservation.IsEnabled = false;
            _databaseService.SaveChanges();
        }

        #endregion

        #region Slot

        public DbQuery<SlotReservation> SlotReservations => _databaseService.SlotReservations.AsNoTracking();

        public void InsertSlotReservation(Slot slot, DateRange range = null)
        {
            if (slot == null)
                throw new ArgumentException(nameof(slot));

            if (range == null)
                range = DateRange.Unspecified;

            _databaseService.SlotReservations.Add(new SlotReservation
            {
                Reservation = new Reservation
                {
                    IsEnabled = true,
                    Range = range
                },
                Slot = slot
            });
            _databaseService.SaveChanges();
        }

        public void UpdateSlotReservation(SlotReservation slotReservation)
        {
            var rsv = _databaseService.Reservations.SingleOrDefault(w => w.Id == slotReservation.Reservation.Id);
            if (rsv == null)
                throw new InvalidOperationException();
            rsv.SlotReservation.Merge(slotReservation);
            _databaseService.SaveChanges();
        }

        public void DeleteSlotReservation(SlotReservation slotReservation)
        {
            slotReservation.Reservation.IsEnabled = false;
            _databaseService.SaveChanges();
        }

        #endregion

        #region Slot2

        public DbQuery<SlotReservation2> SlotReservations2 => _databaseService.SlotReservations2.AsNoTracking();

        public void InsertSlotReservation2(string slotId, DateRange range = null)
        {
            if (string.IsNullOrWhiteSpace(slotId))
                throw new ArgumentException(nameof(slotId));

            if (range == null)
                range = DateRange.Unspecified;

            _databaseService.SlotReservations2.Add(new SlotReservation2
            {
                Reservation = new Reservation
                {
                    IsEnabled = true,
                    Range = range
                },
                SlotId = slotId
            });
            _databaseService.SaveChanges();
        }

        public void UpdateSlotReservation2(SlotReservation2 slotReservation2)
        {
            var rsv = _databaseService.Reservations.SingleOrDefault(w => w.Id == slotReservation2.Reservation.Id);
            if (rsv == null)
                throw new InvalidOperationException();
            rsv.SlotReservation2.Merge(slotReservation2);
            _databaseService.SaveChanges();
        }

        public void DeleteSlotReservation2(SlotReservation2 slotReservation2)
        {
            slotReservation2.Reservation.IsEnabled = false;
            _databaseService.SaveChanges();
        }

        #endregion
    }
}