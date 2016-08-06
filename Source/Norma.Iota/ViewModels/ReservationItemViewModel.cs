using Norma.Iota.Models;

namespace Norma.Iota.ViewModels
{
    internal class ReservationItemViewModel
    {
        public ReservationItem ReservationItem { get; }

        public string Type => ReservationItem.Type;
        public string Title => ReservationItem.Title ?? "-";
        public string StartAt => ReservationItem.StartAt?.ToString("g") ?? "-";
        public string Condition => ReservationItem.Condition;
        public bool IsEditable => ReservationItem.IsEditable;

        public ReservationItemViewModel(ReservationItem reservationItem)
        {
            ReservationItem = reservationItem;
        }
    }
}