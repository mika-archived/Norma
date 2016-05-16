using Norma.Extensions;
using Norma.Models;
using Norma.ViewModels.Internal;
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
            OperationViewModel = new OperationViewModel().AddTo(this);
            OthersViewModel = new OthersViewModel().AddTo(this);
        }
    }
}