using System;
using System.Linq;

using Microsoft.Practices.ServiceLocation;

using Norma.Delta.Models;
using Norma.Delta.Services;

namespace Norma.Iota.Models
{
    internal class ReservationItem
    {
        private readonly DatabaseService _databaseService;
        private readonly KeywordReservation _keywordReservation;
        private readonly SeriesReservation _seriesReservation;
        private readonly SlotReservation _slotReservation;
        private readonly SlotReservation2 _slotReservation2;
        private readonly TimeReservation _timeReservation;

        public string Type { get; private set; }
        public string Title { get; private set; }
        public DateTime? StartAt { get; private set; }

        private ReservationItem()
        {
            _keywordReservation = null;
            _seriesReservation = null;
            _slotReservation = null;
            _slotReservation2 = null;
            _timeReservation = null;
            _databaseService = ServiceLocator.Current.GetInstance<DatabaseService>();
            StartAt = null;
            Title = null;
        }

        public ReservationItem(Reservation reservation) : this()
        {
            if (reservation.KeywordReservation != null)
            {
                _keywordReservation = reservation.KeywordReservation;
                AttachKeywordReservation();
            }
            else if (reservation.SeriesReservation != null)
            {
                _seriesReservation = reservation.SeriesReservation;
                AttachSeriesReservation();
            }
            else if (reservation.SlotReservation != null)
            {
                _slotReservation = reservation.SlotReservation;
                AttachSlotReservation();
            }
            else if (reservation.SlotReservation2 != null)
            {
                _slotReservation2 = reservation.SlotReservation2;
                AttachSlotReservation2();
            }
            else if (reservation.TimeReservation != null)
                _timeReservation = reservation.TimeReservation;
            else
                throw new NotSupportedException();
        }

        private void AttachKeywordReservation()
        {
            Type = "キーワード";
            Title = $"{_keywordReservation.Keyword} {(_keywordReservation.IsRegex ? "(正規表現モード)" : "")}".Trim();
        }

        private void AttachSeriesReservation()
        {
            var wrapSeries = new WrapSeries(_seriesReservation.Series);
            Type = "シリーズ";
            Title = wrapSeries.SeriesName;
        }

        private void AttachSlotReservation()
        {
            Type = "単体";
            StartAt = _slotReservation.Slot.StartAt;
            Title = _slotReservation.Slot.Title;
        }

        private void AttachSlotReservation2()
        {
            Type = "単体";
            using (var connection = _databaseService.Connect())
            {
                var slot = connection.Slots.SingleOrDefault(w => w.SlotId == _slotReservation2.SlotId);
                StartAt = slot?.StartAt;
                Title = slot?.Title;
            }
        }

        private void AttachTimeReservation()
        {
            Type = "時間";
            StartAt = _timeReservation.StartAt;
        }
    }
}