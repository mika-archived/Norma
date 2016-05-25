using Norma.Models;
using Norma.ViewModels.Internal;
using Norma.ViewModels.Tabs;

namespace Norma.ViewModels.WindowContents
{
    internal class SettingsContentViewModel : ViewModel
    {
        public OptionsTabViewModel OptionsTabViewModel { get; }

        public SettingsContentViewModel(Configuration configuration)
        {
            OptionsTabViewModel = new OptionsTabViewModel(configuration);
        }
    }
}