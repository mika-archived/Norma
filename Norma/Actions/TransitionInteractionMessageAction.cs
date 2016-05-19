using System;
using System.Windows;
using System.Windows.Interactivity;

using Norma.Interactivity;

namespace Norma.Actions
{
    // named by Livet
    internal class TransitionInteractionMessageAction : TriggerAction<DependencyObject>
    {
        public static readonly DependencyProperty IsModalProperty =
            DependencyProperty.Register(nameof(IsModal), typeof(bool), typeof(TransitionInteractionMessageAction), null);

        public bool IsModal
        {
            get { return (bool) GetValue(IsModalProperty); }
            set { SetValue(IsModalProperty, value); }
        }

        #region Overrides of TriggerAction

        protected override void Invoke(object parameter)
        {
            var args = parameter as InteractionRequestedEventArgs2;
            var windowType = args?.Context as Type;
            if (windowType == null)
                return;
            var window = (Window) Activator.CreateInstance(windowType);
            if (IsModal)
            {
                window.Owner = Window.GetWindow(AssociatedObject);
                window.ShowDialog();
            }
            else
                window.Show();
        }

        #endregion
    }
}