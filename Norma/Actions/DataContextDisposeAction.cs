using System;
using System.Windows;
using System.Windows.Interactivity;

namespace Norma.Actions
{
    internal class DataContextDisposeAction : TriggerAction<FrameworkElement>
    {
        #region Overrides of TriggerAction

        protected override void Invoke(object parameter)
        {
            var disposable = AssociatedObject.DataContext as IDisposable;
            disposable?.Dispose();
        }

        #endregion
    }
}