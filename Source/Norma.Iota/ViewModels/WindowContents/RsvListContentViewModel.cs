using Norma.Eta.Models;
using Norma.Eta.Mvvm;
using Norma.Eta.Notifications;
using Norma.Eta.Properties;

namespace Norma.Iota.ViewModels.WindowContents
{
    internal class RsvListContentViewModel : InteractionViewModel<DataPassingNotification>
    {
        private Reservation _reservation;
        public string WindowTitle => Resources.RsvList;

        public RsvListContentViewModel(Reservation reservation)
        {
            _reservation = reservation;
        }
    }
}