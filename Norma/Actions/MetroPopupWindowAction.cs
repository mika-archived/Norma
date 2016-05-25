using System;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

using MetroRadiance.Chrome;
using MetroRadiance.UI.Controls;

using Prism.Interactivity;
using Prism.Interactivity.InteractionRequest;

namespace Norma.Actions
{
    internal class MetroPopupWindowAction : PopupWindowAction
    {
        // ReSharper disable once InconsistentNaming
        public static readonly DependencyProperty NewUIThreadProperty
            = DependencyProperty.Register(nameof(NewUIThread), typeof(bool), typeof(MetroPopupWindowAction));

        // ReSharper disable once InconsistentNaming
        public bool NewUIThread
        {
            get { return (bool) GetValue(NewUIThreadProperty); }
            set { SetValue(NewUIThreadProperty, value); }
        }

        #region Overrides of PopupWindowAction

        // If NewUIThread = true, launch new UI thread.
        // Base: https://github.com/PrismLibrary/Prism/blob/master/Source/Wpf/Prism.Wpf/Interactivity/PopupWindowAction.cs#L116-L197
        protected override void Invoke(object parameter)
        {
            if (!NewUIThread)
            {
                base.Invoke(parameter);
                return;
            }

            var thread = new Thread(() =>
            {
                var dispatcherSyncContext = new DispatcherSynchronizationContext(Dispatcher.CurrentDispatcher);
                SynchronizationContext.SetSynchronizationContext(dispatcherSyncContext);

                base.Invoke(parameter);
                Dispatcher.Run();
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.IsBackground = true;
            thread.Start();
        }

        protected override Window GetWindow(INotification notification)
        {
            if (!NewUIThread)
                return base.GetWindow(notification);
            var window = base.GetWindow(notification);

            EventHandler handler = null;
            handler = (sender, e) =>
            {
                window.Closed -= handler;
                Dispatcher.CurrentDispatcher.BeginInvokeShutdown(DispatcherPriority.Background);
            };
            window.Closed += handler;
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