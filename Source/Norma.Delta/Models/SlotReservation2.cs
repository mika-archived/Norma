using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Norma.Delta.Models
{
    public class SlotReservation2
    {
        public string SlotId { get; set; }

        [Key]
        [ForeignKey(nameof(Reservation))]
        public int ReservationId { get; set; }

        public virtual Reservation Reservation { get; set; }

        public void Merge(SlotReservation2 slotReservation2)
        {
            SlotId = slotReservation2.SlotId;
            ReservationId = slotReservation2.ReservationId;
            Reservation.Merge(slotReservation2.Reservation);
        }
    }
}