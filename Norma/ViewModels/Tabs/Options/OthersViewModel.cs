using MetroRadiance.UI;

using Norma.Extensions;
using Norma.Models.Config;
using Norma.ViewModels.Internal;

using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace Norma.ViewModels.Tabs.Options
{
    internal class OthersViewModel : ViewModel
    {
        public ReactiveProperty<Theme.SpecifiedColor> SelectedTheme { get; private set; }

        public OthersViewModel(OthersConfig oc)
        {
            SelectedTheme = oc.ToReactivePropertyAsSynchronized(w => w.Theme).AddTo(this);
        }
    }
}