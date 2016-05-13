using System;
using System.ComponentModel;
using System.Reactive.Linq;

using Prism.Mvvm;

namespace Norma.Helpers
{
    internal static class ViewModelHelper
    {
        public static IDisposable Subscribe(this BindableBase notifyPropertyChanged, string propertyName, Action<PropertyChangedEventArgs> action)
        {
            return Observable.FromEvent<PropertyChangedEventHandler, PropertyChangedEventArgs>(
                handler => (sender, e) => handler(e),
                handler => notifyPropertyChanged.PropertyChanged += handler,
                handler => notifyPropertyChanged.PropertyChanged -= handler)
                .Where(e => e.PropertyName == propertyName)
                .Subscribe(action.Invoke);
        }
    }
}