using System;

using Norma.ViewModels.Internal;

namespace Norma.Extensions
{
    // ReSharper disable once InconsistentNaming
    internal static class IDisposableExt
    {
        public static void AddTo(this IDisposable disposable, ViewModel viewmodel)
        {
            viewmodel.CompositeDisposable.Add(disposable);
        }
    }
}