namespace Norma.Delta.Models
{
    public class KeywordReservation
    {
        public string Keyword { get; set; }

        public bool IsRegex { get; set; }

        public virtual Reservation Reservation { get; set; }
    }
}