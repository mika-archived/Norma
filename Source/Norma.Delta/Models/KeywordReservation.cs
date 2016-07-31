using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Norma.Delta.Models
{
    public class KeywordReservation
    {
        public string Keyword { get; set; }

        public bool IsRegex { get; set; }

        [Key]
        [ForeignKey(nameof(Reservation))]
        public int ReservationId { get; set; }

        public virtual Reservation Reservation { get; set; }

        public void Merge(KeywordReservation keywordReservation)
        {
            Keyword = keywordReservation.Keyword;
            IsRegex = keywordReservation.IsRegex;
            ReservationId = keywordReservation.ReservationId;
            Reservation.Merge(keywordReservation.Reservation);
        }
    }
}