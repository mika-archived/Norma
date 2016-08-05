using Norma.Eta.Mvvm;
using Norma.Eta.Notifications;
using Norma.Eta.Properties;

namespace Norma.Iota.ViewModels.WindowContents
{
    internal class ConditionalReservationContentViewModel : InteractionViewModel<DataPassingNotification>
    {
        public string WindowTitle => Resources.ConditionalReservation;

        public ConditionalReservationContentViewModel()
        {
            ViewModelHelper.Subscribe(this, w => w.Notification, w => Reset());
        }

        private void Reset()
        {

        }
    }
}