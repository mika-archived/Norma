namespace Norma.Delta.Models
{
    public class SlotReservation
    {
        public virtual Slot Slot { get; set; }

        public virtual Reservation Reservation { get; set; }
    }
}