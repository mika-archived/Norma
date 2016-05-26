using System.Windows;
using System.Windows.Media;

using MetroRadiance.Chrome;
using MetroRadiance.UI.Controls;

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
            var window = base.GetWindow(notification);
            if (CenterOverAssociatedObject)
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

        #endregion
    }
}