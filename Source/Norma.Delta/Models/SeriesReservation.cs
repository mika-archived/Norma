namespace Norma.Delta.Models
{
    public class SeriesReservation
    {
        public virtual Series Series { get; set; }

        public virtual Reservation Reservation { get; set; }
    }
}