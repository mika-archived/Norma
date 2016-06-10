using Norma.Eta.Models;
using Norma.Eta.Mvvm;
using Norma.ViewModels.Tabs.Options;

namespace Norma.ViewModels.Tabs
{
    internal class OptionsTabViewModel : ViewModel
    {
        public BrowserViewModel BrowserViewModel { get; }
        public OperationViewModel OperationViewModel { get; }
        public OthersViewModel OthersViewModel { get; }

        public OptionsTabViewModel(Configuration configuration)
        {
            BrowserViewModel = new BrowserViewModel(configuration.Root.Browser).AddTo(this);
            OperationViewModel = new OperationViewModel(configuration.Root.Operation).AddTo(this);
            OthersViewModel = new OthersViewModel(configuration.Root.Others).AddTo(this);
        }
    }
}