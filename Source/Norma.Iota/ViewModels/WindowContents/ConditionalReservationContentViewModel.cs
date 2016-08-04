using Norma.Eta.Mvvm;
using Norma.Eta.Notifications;
using Norma.Eta.Properties;

namespace Norma.Iota.ViewModels.WindowContents
{
    internal class ConditionalReservationContentViewModel : InteractionViewModel<DataPassingNotification>
    {
        public string WindowTitle => Resources.ConditionalReservation;
    }
}