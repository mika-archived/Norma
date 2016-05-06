using System;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Threading;

using Prism.Mvvm;

namespace Norma.ViewModels.Internal
{
    internal class ViewModel : BindableBase, IDisposable
    {
        protected internal CompositeDisposable CompositeDisposable { get; }
        protected Dispatcher Dispatcher { get; }

        protected ViewModel()
        {
            CompositeDisposable = new CompositeDisposable();
            Dispatcher = Application.Current.MainWindow.Dispatcher;
        }

        #region Implementation of IDisposable

        public virtual void Dispose()
        {
            CompositeDisposable.Dispose();
        }

        #endregion
    }
}