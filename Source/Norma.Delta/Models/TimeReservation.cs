using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Norma.Delta.Models.Enums;

namespace Norma.Delta.Models
{
    public class TimeReservation
    {
        public DateTime StartAt { get; set; }

        public Repetition Repetition { get; set; }

        [Key]
        [ForeignKey(nameof(Reservation))]
        public int ReservationId { get; set; }

        public virtual Reservation Reservation { get; set; }

        public void Merge(TimeReservation timeReservation)
        {
            StartAt = timeReservation.StartAt;
            Repetition = timeReservation.Repetition;
            Reservation.Merge(timeReservation.Reservation);
        }
    }
}