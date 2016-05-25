using Prism.Interactivity.InteractionRequest;

namespace Norma.Notifications
{
    internal class DataPassingNotification : Notification
    {
        public object Model { get; set; } = null;
    }
}