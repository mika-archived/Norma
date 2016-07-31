using Norma.Eta.Models;

namespace Norma.Delta.Models
{
    public class Reservation
    {
        public int Id { get; set; }

        public bool IsEnabled { get; set; }

        public virtual DateRange Range { get; set; }

        public virtual KeywordReservation KeywordReservation { get; set; }

        public virtual TimeReservation TimeReservation { get; set; }

        public virtual SeriesReservation SeriesReservation { get; set; }

        public virtual SlotReservation SlotReservation { get; set; }
    }
}