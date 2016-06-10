using System.Windows;
using System.Windows.Media;

using MetroRadiance.Chrome;
using MetroRadiance.UI.Controls;

using Norma.Eta.PopupWindows;

using Prism.Interactivity;
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
            Window wrapperWindow;
            if (WindowContent != null)
            {
                wrapperWindow = CreateWindow();
                wrapperWindow.DataContext = notification;
                wrapperWindow.Title = notification.Title;
                PrepareContentForWindow(notification, wrapperWindow);
            }
            else
                wrapperWindow = CreateDefaultMetroWindow(notification);
            if (AssociatedObject != null)
                wrapperWindow.Owner = Window.GetWindow(AssociatedObject);
            if (WindowStyle != null)
                wrapperWindow.Style = WindowStyle;
            if (CenterOverAssociatedObject && AssociatedObject != null)
            {
                wrapperWindow.Owner = Window.GetWindow(AssociatedObject);
                wrapperWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                CenterOverAssociatedObject = false;
            }
            return wrapperWindow;
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
                return new DefaultConfirmationMetroWindow {Confirmation = (IConfirmation) notification};
            return new DefaultNotificationMetroWindow {Notification = notification};
        }

        #endregion
    }
}