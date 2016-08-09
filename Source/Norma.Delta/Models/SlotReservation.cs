using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Norma.Delta.Models
{
    public class SlotReservation
    {
        public virtual Slot Slot { get; set; }

        [Key]
        [ForeignKey(nameof(Reservation))]
        public int ReservationId { get; set; }

        public virtual Reservation Reservation { get; set; }

        public void Merge(SlotReservation slotReservation)
        {
            Reservation.Merge(slotReservation.Reservation);
        }
    }
}