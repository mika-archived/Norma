using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Norma.Delta.Models
{
    public class SeriesReservation
    {
        public virtual Series Series { get; set; }

        [Key]
        [ForeignKey(nameof(Reservation))]
        public int ReservationId { get; set; }

        public virtual Reservation Reservation { get; set; }

        public void Merge(SeriesReservation seriesReservation)
        {
            Series = seriesReservation.Series;
            ReservationId = seriesReservation.ReservationId;
            Reservation.Merge(seriesReservation.Reservation);
        }
    }
}