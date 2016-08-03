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

        public string ProgramId { get; private set; }
        public string Title => _model.Title;
        public DateTime StartAt => _model.StartAt;
        public DateTime EndAt => _model.EndAt;
        public DateTime FixedStartAt { get; }
        public DateTime FixedEndAt { get; }
        public bool CanReservation { get; }
        public Channel Channel { get; private set; }
        public string Highlight => string.IsNullOrWhiteSpace(_model.Highlight) ? _model.HighlightDetail : _model.Highlight;
        public string Description => _model.Description;
        public List<string> Casts { get; }
        public List<string> Crews { get; }

        public WrapSlot(Slot slot, DateTime date)
        {
            _model = slot;
            FixedStartAt = _model.StartAt < date ? new DateTime(date.Year, date.Month, date.Day) : _model.StartAt;
            FixedEndAt = _model.EndAt >= date.AddDays(1)
                ? new DateTime(date.Year, date.Month, date.Day, 23, 59, 59)
                : _model.EndAt;
            Casts = new List<string>();
            Crews = new List<string>();
            CanReservation = FixedStartAt > DateTime.Now.AddMinutes(5);
        }

        public void RequestDetails()
        {
            var databaseService = ServiceLocator.Current.GetInstance<DatabaseService>();
            using (var connection = databaseService.Connect())
            {
                var slot = connection.Slots.AsNoTracking().Single(w => w.SlotId == _model.SlotId);
                slot.Episodes.First().Casts.ToList().ForEach(w => Casts.Add(w.Name));
                slot.Episodes.First().Crews.ToList().ForEach(w => Crews.Add(w.Name));
                ProgramId = slot.Episodes.First().EpisodeId;
                Channel = slot.Channel;
            }
        }
    }
}