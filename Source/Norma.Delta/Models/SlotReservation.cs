namespace Norma.Delta.Models
{
    public class SlotReservation
    {
        public virtual Slot Slot { get; set; }

        public virtual Reservation Reservation { get; set; }

        public void Merge(SlotReservation slotReservation)
        {
            Slot = slotReservation.Slot;
            Reservation.Merge(slotReservation.Reservation);
        }
    }
}