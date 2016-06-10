using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reactive.Linq;

using Prism.Mvvm;

namespace Norma.Eta.Mvvm
{
    public static class ViewModelHelper
    {
        public static IDisposable Subscribe<T>(T vm, Expression<Func<T, object>> expr, Action<PropertyChangedEventArgs> action) where T : BindableBase
        {
            return Observable.FromEvent<PropertyChangedEventHandler, PropertyChangedEventArgs>(
                handler => (sender, e) => handler(e),
                handler => vm.PropertyChanged += handler,
                handler => vm.PropertyChanged -= handler)
                .Where(e => e.PropertyName == ((MemberExpression)expr.Body).Member.Name)
                .Subscribe(action.Invoke);
        }
    }
}