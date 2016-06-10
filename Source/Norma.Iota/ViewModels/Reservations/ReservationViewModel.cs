using Norma.Eta.Models.Reservations;

namespace Norma.Iota.ViewModels.Reservations
{
    internal class ReservationViewModel
    {
        protected Reserve Reserve { get; }

        protected ReservationViewModel(Reserve reserve)
        {
            Reserve = reserve;
        }
    }
}