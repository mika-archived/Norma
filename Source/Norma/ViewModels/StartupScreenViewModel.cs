using Microsoft.Practices.ServiceLocation;

using Norma.Eta.Mvvm;
using Norma.Eta.Services;
using Norma.Models;

using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace Norma.ViewModels
{
    internal class StartupScreenViewModel : ViewModel
    {
        public string Name => ProductInfo.Name;

        public ReadOnlyReactiveProperty<string> Text { get; }

        public string Version => $"Version {ProductInfo.Version} {ProductInfo.ReleaseType.ToVersionString()}".Trim();

        public string Copyright => ProductInfo.Copyright;

        public StartupScreenViewModel()
        {
            var statusService = ServiceLocator.Current.GetInstance<StatusService>();
            Text = statusService.ObserveProperty(w => w.Status).ToReadOnlyReactiveProperty().AddTo(this);
        }
    }
}