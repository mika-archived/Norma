using System;
using System.Threading;
using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Threading;

using Norma.Notifications;

using Prism.Interactivity.InteractionRequest;

namespace Norma.Actions
{
    internal class MultiThreadPopupWindowAction : TriggerAction<FrameworkElement>
    {
        public static readonly DependencyProperty IsModalProperty =
            DependencyProperty.Register(nameof(IsModal), typeof(bool), typeof(MultiThreadPopupWindowAction));

        public bool IsModal
        {
            get { return (bool) GetValue(IsModalProperty); }
            set { SetValue(IsModalProperty, value); }
        }

        #region Overrides of TriggerAction

        protected override void Invoke(object parameter)
        {
            var args = parameter as InteractionRequestedEventArgs;
            var notification = args?.Context as WindowNotification;
            if (notification == null)
                return;
            var isModal = IsModal;

            var thread = new Thread(() =>
            {
                var dispatcherSyncContext = new DispatcherSynchronizationContext(Dispatcher.CurrentDispatcher);
                SynchronizationContext.SetSynchronizationContext(dispatcherSyncContext);

                var window = (Window) Activator.CreateInstance(notification.WindowType);
                if (notification.ViewModel != null)
                    window.DataContext = notification.ViewModel;

                EventHandler handler = null;
                handler = (sender, e) =>
                {
                    window.Closed -= handler;
                    window.DataContext = null;
                    window = null;
                    Dispatcher.CurrentDispatcher.BeginInvokeShutdown(DispatcherPriority.Background);
                };
                window.Closed += handler;

                if (isModal)
                    window.ShowDialog();
                else
                    window.Show();

                Dispatcher.Run();
            });
            thread.SetApartmentState(ApartmentState.STA);
            // thread.IsBackground = true;
            thread.Start();
        }

        #endregion
    }
}