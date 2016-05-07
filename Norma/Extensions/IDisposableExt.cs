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

        public static T AddTo<T>(this T disposable, ViewModel viewmodel) where T : IDisposable
        {
            viewmodel.CompositeDisposable.Add(disposable);
            return disposable;
        }
    }
}