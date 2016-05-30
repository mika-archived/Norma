using System.Windows;
using System.Windows.Media;

using MetroRadiance.Chrome;
using MetroRadiance.UI.Controls;

using Norma.Eta.PopupWindows;

using Prism.Interactivity;
using Prism.Interactivity.DefaultPopupWindows;
using Prism.Interactivity.InteractionRequest;

namespace Norma.Eta.Actions
{
    public class MetroPopupWindowAction : PopupWindowAction
    {
        #region Overrides of PopupWindowAction

        protected override void Invoke(object parameter)
        {
            base.Invoke(parameter);
            CenterOverAssociatedObject = true;
        }

        protected override Window GetWindow(INotification notification)
        {
            var window = base.GetWindow(notification);
            if (window is DefaultNotificationWindow)
            {
                window = CreateDefaultMetroWindow(notification);
                if (AssociatedObject != null)
                    window.Owner = Window.GetWindow(AssociatedObject);
                if (WindowStyle != null)
                    window.Style = WindowStyle;
            }
            if (CenterOverAssociatedObject && AssociatedObject != null)
            {
                window.Owner = Window.GetWindow(AssociatedObject);
                window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                CenterOverAssociatedObject = false;
            }
            return window;
        }

        protected override Window CreateWindow()
        {
            var window = new MetroWindow
            {
                Style = new Style(),
                FontFamily = new FontFamily("Segoe UI"),
                UseLayoutRounding = true
            };
            TextOptions.SetTextFormattingMode(window, TextFormattingMode.Display);
            WindowChrome.SetInstance(window, new WindowChrome());
            return window;
        }

        private Window CreateDefaultMetroWindow(INotification notification)
        {
            if (notification is IConfirmation)
                return CreateDefaultWindow(notification);
            return new DefaultNotificationMetroWindow {Notification = notification};
        }

        #endregion
    }
}