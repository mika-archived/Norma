using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Practices.ServiceLocation;

using Norma.Delta.Models;
using Norma.Delta.Services;

namespace Norma.Iota.Models
{
    internal class WrapSlot
    {
        private readonly Slot _model;

        public string Title => _model.Title;
        public DateTime StartAt => _model.StartAt;
        public DateTime EndAt => _model.EndAt;
        public DateTime FixedStartAt { get; }
        public DateTime FixedEndAt { get; }
        public string Highlight => string.IsNullOrWhiteSpace(_model.Highlight) ? _model.HighlightDetail : _model.Highlight;
        public string Description => _model.Description;

        public bool CanSlotReservation { get; private set; }
        public bool CanSeriesReservation { get; private set; }
        public Channel Channel { get; private set; }
        public List<string> Casts { get; }
        public List<string> Crews { get; }
        public Slot Slot { get; private set; }
        public Series Series { get; private set; }
        public string ProgramId { get; private set; }

        public WrapSlot(Slot slot, DateTime date)
        {
            _model = slot;
            FixedStartAt = _model.StartAt < date ? new DateTime(date.Year, date.Month, date.Day) : _model.StartAt;
            FixedEndAt = _model.EndAt >= date.AddDays(1)
                ? new DateTime(date.Year, date.Month, date.Day, 23, 59, 59)
                : _model.EndAt;
            Casts = new List<string>();
            Crews = new List<string>();
            CanSlotReservation = FixedStartAt > DateTime.Now.AddMinutes(5);
            if (slot.Channel != null)
                Channel = slot.Channel;
        }

        public void RequestDetails()
        {
            var databaseService = ServiceLocator.Current.GetInstance<DatabaseService>();
            using (var connection = databaseService.Connect())
            {
                Slot = connection.Slots.Single(w => w.SlotId == _model.SlotId);
                var firstEpisode = Slot.Episodes.First();
                firstEpisode.Casts.ToList().ForEach(w => Casts.Add(w.Name));
                firstEpisode.Crews.ToList().ForEach(w => Crews.Add(w.Name));
                ProgramId = firstEpisode.EpisodeId;
                Channel = Slot.Channel;
                Series = firstEpisode.Series;
                if (CanSlotReservation)
                    CanSlotReservation = !connection.SlotReservations.Any(w => w.Slot.SlotId == _model.SlotId);
                CanSeriesReservation = !connection.SeriesReservations.Any(w => w.Series.SeriesId == Series.SeriesId);
            }
        }
    }
}