using Norma.Extensions;
using Norma.ViewModels.Internal;
using Norma.ViewModels.Tabs.Options;

namespace Norma.ViewModels.Tabs
{
    internal class OptionsTabViewModel : ViewModel
    {
        public BrowserViewModel BrowserViewModel { get; }
        public OperationViewModel OperationViewModel { get; }
        public OthersViewModel OthersViewModel { get; }

        public OptionsTabViewModel()
        {
            BrowserViewModel = new BrowserViewModel().AddTo(this);
            OperationViewModel = new OperationViewModel().AddTo(this);
            OthersViewModel = new OthersViewModel().AddTo(this);
        }

        public void Save()
        {

        }
    }
}