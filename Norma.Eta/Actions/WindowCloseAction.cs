using System.Windows;
using System.Windows.Interactivity;

namespace Norma.Eta.Actions
{
    public class WindowCloseAction : TriggerAction<FrameworkElement>
    {
        #region Overrides of TriggerAction

        protected override void Invoke(object parameter)
        {
            var window = Window.GetWindow(AssociatedObject);
            window?.Close();
        }

        #endregion
    }
}