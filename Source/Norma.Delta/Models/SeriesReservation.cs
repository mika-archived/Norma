namespace Norma.Delta.Models
{
    public class SeriesReservation
    {
        public virtual Series Series { get; set; }

        public virtual Reservation Reservation { get; set; }

        public void Merge(SeriesReservation seriesReservation)
        {
            Series = seriesReservation.Series;
            Reservation.Merge(seriesReservation.Reservation);
        }
    }
}