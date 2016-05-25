using System;
using System.ComponentModel;
using System.Reactive.Linq;

using Prism.Mvvm;

namespace Norma.Helpers
{
    internal static class ViewModelHelper
    {
        public static IDisposable Subscribe(BindableBase vm, string propertyName, Action<PropertyChangedEventArgs> action)
        {
            return Observable.FromEvent<PropertyChangedEventHandler, PropertyChangedEventArgs>(
                handler => (sender, e) => handler(e),
                handler => vm.PropertyChanged += handler,
                handler => vm.PropertyChanged -= handler)
                .Where(e => e.PropertyName == propertyName)
                .Subscribe(action.Invoke);
        }
    }
}