using System;

namespace Norma.Eta.Mvvm
{
    // ReSharper disable once InconsistentNaming
    public static class IDisposableExt
    {
        public static T AddTo<T>(this T disposable, ViewModel viewModel) where T : IDisposable
        {
            viewModel.CompositeDisposable.Add(disposable);
            return disposable;
        }
    }
}