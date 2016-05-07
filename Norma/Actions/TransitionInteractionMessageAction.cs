using System;
using System.Windows;
using System.Windows.Interactivity;

using Norma.Interactivity;

namespace Norma.Actions
{
    // named by Livet
    internal class TransitionInteractionMessageAction : TriggerAction<DependencyObject>
    {
        #region Overrides of TriggerAction

        protected override void Invoke(object parameter)
        {
            var args = parameter as InteractionRequestedEventArgs2;
            var windowType = args?.Context as Type;
            if (windowType == null)
                return;
            var window = (Window) Activator.CreateInstance(windowType);
            window.Show();
        }

        #endregion
    }
}