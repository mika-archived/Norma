namespace Norma.Delta.Models
{
    public class SlotReservation2
    {
        public string SlotId { get; set; }

        public virtual Reservation Reservation { get; set; }

        public void Merge(SlotReservation2 slotReservation2)
        {
            SlotId = slotReservation2.SlotId;
            Reservation.Merge(slotReservation2.Reservation);
        }
    }
}