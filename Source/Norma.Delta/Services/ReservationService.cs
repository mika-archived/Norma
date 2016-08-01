using System;
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

        public void InsertTimeReservaion(DateTime startAt, Repetition repetition, DateRange range = null)
        {
            if (range == null)
                range = DateRange.Unspecified;

            using (var connection = _databaseService.Connect())
            {
                connection.TimeReservations.Add(new TimeReservation
                {
                    Reservation = new Reservation
                    {
                        IsEnabled = true,
                        Range = range
                    },
                    StartAt = startAt,
                    Repetition = repetition
                });
                connection.SaveChanges();
            }
        }

        public void UpdateTimeReservation(TimeReservation timeReservation)
        {
            using (var connection = _databaseService.Connect())
            {
                var rsv = connection.Reservations.SingleOrDefault(w => w.ReservationId == timeReservation.Reservation.ReservationId);
                if (rsv == null)
                    throw new InvalidOperationException();
                rsv.TimeReservation.Merge(timeReservation);
                connection.DetectChanges();
                connection.SaveChanges();
            }
        }

        public void DeleteTimeReservation(TimeReservation timeReservation)
        {
            timeReservation.Reservation.IsEnabled = false;
            UpdateTimeReservation(timeReservation);
        }

        #endregion

        #region Keyword

        public void InsertKeywordReservation(string keyword, bool isRegex, DateRange range = null)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                throw new ArgumentException(nameof(keyword));

            if (range == null)
                range = DateRange.Unspecified;

            using (var connection = _databaseService.Connect())
            {
                connection.KeywordReservations.Add(new KeywordReservation
                {
                    Reservation = new Reservation
                    {
                        IsEnabled = true,
                        Range = range
                    },
                    Keyword = keyword,
                    IsRegex = isRegex
                });
                connection.SaveChanges();
            }
        }

        public void UpdateKeywordReservation(KeywordReservation keywordReservation)
        {
            using (var connection = _databaseService.Connect())
            {
                var rsv = connection.Reservations.SingleOrDefault(w => w.ReservationId == keywordReservation.Reservation.ReservationId);
                if (rsv == null)
                    throw new InvalidOperationException();
                connection.DetectChanges();
                rsv.KeywordReservation.Merge(keywordReservation);
                connection.SaveChanges();
            }
        }

        public void DeleteKeywordReservation(KeywordReservation keywordReservation)
        {
            keywordReservation.Reservation.IsEnabled = false;
            UpdateKeywordReservation(keywordReservation);
        }

        #endregion

        #region Series

        public void InsertSeriesReservation(Series series, DateRange range = null)
        {
            if (series == null)
                throw new ArgumentException(nameof(series));

            if (range == null)
                range = DateRange.Unspecified;

            using (var connection = _databaseService.Connect())
            {
                connection.SeriesReservations.Add(new SeriesReservation
                {
                    Reservation = new Reservation
                    {
                        IsEnabled = true,
                        Range = range
                    },
                    Series = series
                });
                connection.SaveChanges();
            }
        }

        public void UpdateSeriesReservation(SeriesReservation seriesReservation)
        {
            using (var connection = _databaseService.Connect())
            {
                var rsv = connection.Reservations.SingleOrDefault(w => w.ReservationId == seriesReservation.Reservation.ReservationId);
                if (rsv == null)
                    throw new InvalidOperationException();

                rsv.SeriesReservation.Merge(seriesReservation);
                connection.DetectChanges();
                connection.SaveChanges();
            }
        }

        public void DeleteSeriesReservation(SeriesReservation seriesReservation)
        {
            seriesReservation.Reservation.IsEnabled = false;
            UpdateSeriesReservation(seriesReservation);
        }

        #endregion

        #region Slot

        public void InsertSlotReservation(Slot slot, DateRange range = null)
        {
            if (slot == null)
                throw new ArgumentException(nameof(slot));

            if (range == null)
                range = DateRange.Unspecified;

            using (var connection = _databaseService.Connect())
            {
                connection.SlotReservations.Add(new SlotReservation
                {
                    Reservation = new Reservation
                    {
                        IsEnabled = true,
                        Range = range
                    },
                    Slot = slot
                });
                connection.SaveChanges();
            }
        }

        public void UpdateSlotReservation(SlotReservation slotReservation)
        {
            using (var connection = _databaseService.Connect())
            {
                var rsv = connection.Reservations.SingleOrDefault(w => w.ReservationId == slotReservation.Reservation.ReservationId);
                if (rsv == null)
                    throw new InvalidOperationException();
                rsv.SlotReservation.Merge(slotReservation);
                connection.DetectChanges();
                connection.SaveChanges();
            }
        }

        public void DeleteSlotReservation(SlotReservation slotReservation)
        {
            slotReservation.Reservation.IsEnabled = false;
            UpdateSlotReservation(slotReservation);
        }

        #endregion

        #region Slot2

        public void InsertSlotReservation2(string slotId, DateRange range = null)
        {
            if (string.IsNullOrWhiteSpace(slotId))
                throw new ArgumentException(nameof(slotId));

            if (range == null)
                range = DateRange.Unspecified;

            using (var connection = _databaseService.Connect())
            {
                connection.SlotReservations2.Add(new SlotReservation2
                {
                    Reservation = new Reservation
                    {
                        IsEnabled = true,
                        Range = range
                    },
                    SlotId = slotId
                });
                connection.SaveChanges();
            }
        }

        public void UpdateSlotReservation2(SlotReservation2 slotReservation2)
        {
            using (var connection = _databaseService.Connect())
            {
                var rsv = connection.Reservations.SingleOrDefault(w => w.ReservationId == slotReservation2.Reservation.ReservationId);
                if (rsv == null)
                    throw new InvalidOperationException();
                rsv.SlotReservation2.Merge(slotReservation2);
                connection.DetectChanges();
                connection.SaveChanges();
            }
        }

        public void DeleteSlotReservation2(SlotReservation2 slotReservation2)
        {
            slotReservation2.Reservation.IsEnabled = false;
            UpdateSlotReservation2(slotReservation2);
        }

        #endregion
    }
}