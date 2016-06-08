using System.Collections.ObjectModel;

using Norma.Eta.Models;
using Norma.Eta.Mvvm;
using Norma.Eta.Notifications;
using Norma.Eta.Properties;
using Norma.Iota.ViewModels.Reservations;

namespace Norma.Iota.ViewModels.WindowContents
{
    internal class RsvListContentViewModel : InteractionViewModel<DataPassingNotification>
    {
        private readonly Reservation _reservation;
        public string WindowTitle => Resources.RsvList;

        public ObservableCollection<RsvAllViewModel> Reservations { get; }

        public RsvListContentViewModel(Reservation reservation)
        {
            _reservation = reservation;
            Reservations = new ObservableCollection<RsvAllViewModel>();

            ViewModelHelper.Subscribe(this, nameof(Notification), w =>
            {
                Reservations.Clear();
                foreach (var rsv in _reservation.Reservations)
                    Reservations.Add(new RsvAllViewModel(rsv));
            });
        }
    }
}