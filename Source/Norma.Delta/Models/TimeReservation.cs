using System;

using Norma.Eta.Models.Enums;

namespace Norma.Delta.Models
{
    public class TimeReservation
    {
        public DateTime StartAt { get; set; }

        public RepetitionType Repetition { get; set; }

        public virtual Reservation Reservation { get; set; }
    }
}