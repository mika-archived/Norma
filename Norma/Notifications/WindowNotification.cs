using System;

using Norma.ViewModels.Internal;

using Prism.Interactivity.InteractionRequest;

namespace Norma.Notifications
{
    internal class WindowNotification : Notification
    {
        public Type WindowType { get; set; }

        public ViewModel ViewModel { get; set; }

        public WindowNotification()
        {
            Content = "";
            Title = "";
        }
    }
}