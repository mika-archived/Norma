using System;
using System.Linq;

using Microsoft.Practices.ServiceLocation;

using Norma.Delta.Models;
using Norma.Delta.Services;
using Norma.Eta.Extensions;
using Norma.Eta.Properties;

namespace Norma.Iota.Models
{
    internal class ReservationItem
    {
        private readonly DatabaseService _databaseService;

        // どうしようかなぁ

        public KeywordReservation KeywordReservation { get; }
        public ReservationService ReservationService { get; }
        public SeriesReservation SeriesReservation { get; }
        public SlotReservation SlotReservation { get; }
        public SlotReservation2 SlotReservation2 { get; }
        public TimeReservation TimeReservation { get; }

        public string Type { get; private set; }
        public string Title { get; private set; }
        public DateTime? StartAt { get; private set; }
        public string Condition { get; private set; }
        public bool IsEditable { get; private set; }

        private ReservationItem()
        {
            KeywordReservation = null;
            SeriesReservation = null;
            SlotReservation = null;
            SlotReservation2 = null;
            TimeReservation = null;
            _databaseService = ServiceLocator.Current.GetInstance<DatabaseService>();
            ReservationService = ServiceLocator.Current.GetInstance<ReservationService>();
            Type = "";
            Title = null;
            StartAt = null;
            Condition = "";
            IsEditable = false;
        }

        public ReservationItem(Reservation reservation) : this()
        {
            if (reservation.KeywordReservation != null)
            {
                KeywordReservation = reservation.KeywordReservation;
                AttachKeywordReservation();
            }
            else if (reservation.SeriesReservation != null)
            {
                SeriesReservation = reservation.SeriesReservation;
                AttachSeriesReservation();
            }
            else if (reservation.SlotReservation != null)
            {
                SlotReservation = reservation.SlotReservation;
                AttachSlotReservation();
            }
            else if (reservation.SlotReservation2 != null)
            {
                SlotReservation2 = reservation.SlotReservation2;
                AttachSlotReservation2();
            }
            else if (reservation.TimeReservation != null)
            {
                TimeReservation = reservation.TimeReservation;
                AttachTimeReservation();
            }
            else
                throw new NotSupportedException();
        }

        private void AttachKeywordReservation()
        {
            Type = Resources.Keyword;
            Title = $"{KeywordReservation.Keyword} {(KeywordReservation.IsRegex ? $"({Resources.RegexMode})" : "")}".Trim();
            Condition = Resources.PartialMatching;
            IsEditable = true;
        }

        private void AttachSeriesReservation()
        {
            var wrapSeries = new WrapSeries(SeriesReservation.Series);
            Type = Resources.Series;
            Title = wrapSeries.SeriesName;
            Condition = Resources.PerfectMatching;
        }

        private void AttachSlotReservation()
        {
            Type = Resources.Slot;
            StartAt = SlotReservation.Slot.StartAt;
            Title = SlotReservation.Slot.Title;
            Condition = Resources.PerfectMatching;
        }

        private void AttachSlotReservation2()
        {
            Type = Resources.Slot;
            using (var connection = _databaseService.Connect())
            {
                var slot = connection.Slots.SingleOrDefault(w => w.SlotId == SlotReservation2.SlotId);
                StartAt = slot?.StartAt;
                Title = slot?.Title ?? Resources.BlankSlot;
            }
            Condition = Resources.PerfectMatching;
        }

        private void AttachTimeReservation()
        {
            Type = Resources.Time;
            StartAt = TimeReservation.StartAt;
            Condition = TimeReservation.Repetition.ToLocaleString();
            IsEditable = true;
        }

        public void Delete()
        {
            if (KeywordReservation != null)
                ReservationService.DeleteKeywordReservation(KeywordReservation);
            else if (SeriesReservation != null)
                ReservationService.DeleteSeriesReservation(SeriesReservation);
            else if (SlotReservation != null)
                ReservationService.DeleteSlotReservation(SlotReservation);
            else if (SlotReservation2 != null)
                ReservationService.DeleteSlotReservation2(SlotReservation2);
            else if (TimeReservation != null)
                ReservationService.DeleteTimeReservation(TimeReservation);
            else
                throw new NotSupportedException();
        }

        public void Update()
        {
            if (KeywordReservation != null)
                ReservationService.UpdateKeywordReservation(KeywordReservation);
            else if (SeriesReservation != null)
                ReservationService.UpdateSeriesReservation(SeriesReservation);
            else if (SlotReservation != null)
                ReservationService.UpdateSlotReservation(SlotReservation);
            else if (SlotReservation2 != null)
                ReservationService.UpdateSlotReservation2(SlotReservation2);
            else if (TimeReservation != null)
                ReservationService.UpdateTimeReservation(TimeReservation);
            else
                throw new NotSupportedException();
        }
    }
}