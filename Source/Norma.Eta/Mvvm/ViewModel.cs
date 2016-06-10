using System;
using System.Reactive.Disposables;

using Prism.Mvvm;

namespace Norma.Eta.Mvvm
{
    public class ViewModel : BindableBase, IDisposable
    {
        public CompositeDisposable CompositeDisposable { get; }

        protected ViewModel()
        {
            CompositeDisposable = new CompositeDisposable();
        }

        #region Implementation of IDisposable

        public virtual void Dispose()
        {
            CompositeDisposable.Dispose();
        }

        #endregion
    }
}