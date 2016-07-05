using Prism.Interactivity.InteractionRequest;

namespace Norma.Eta.Notifications
{
    public class DataPassingNotification : Notification
    {
        public object Model { get; set; }

        public DataPassingNotification()
        {
            Title = "Default";
            Content = "Default";
        }
    }
}