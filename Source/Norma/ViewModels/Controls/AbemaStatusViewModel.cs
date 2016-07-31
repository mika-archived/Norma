using Norma.Eta.Mvvm;
using Norma.Eta.Services;

using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace Norma.ViewModels.Controls
{
    internal class AbemaStatusViewModel : ViewModel
    {
        public ReadOnlyReactiveProperty<string> Text { get; }

        public AbemaStatusViewModel(StatusService statusService)
        {
            Text = statusService.ObserveProperty(w => w.Status).ToReadOnlyReactiveProperty().AddTo(this);
        }
    }
}