namespace Norma.Delta.Models
{
    public class Reservation
    {
        public int ReservationId { get; set; }

        public bool IsEnabled { get; set; }

        public virtual DateRange Range { get; set; }

        public virtual KeywordReservation KeywordReservation { get; set; }

        public virtual TimeReservation TimeReservation { get; set; }

        public virtual SeriesReservation SeriesReservation { get; set; }

        public virtual SlotReservation SlotReservation { get; set; }

        public virtual SlotReservation2 SlotReservation2 { get; set; }

        public void Merge(Reservation reservation)
        {
            IsEnabled = reservation.IsEnabled;
            Range = reservation.Range;
        }
    }
}