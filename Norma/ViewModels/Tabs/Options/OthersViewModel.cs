using MetroRadiance.UI;

using Norma.Models.Config;
using Norma.ViewModels.Internal;

namespace Norma.ViewModels.Tabs.Options
{
    internal class OthersViewModel : ViewModel
    {
        private readonly OthersConfig _othersConfig;

        public OthersViewModel(OthersConfig oc)
        {
            _othersConfig = oc;
            IsDarkChecked = oc.Theme == Theme.SpecifiedColor.Dark;
            IsLightChecked = !IsDarkChecked;
        }

        #region IsDarkChecked

        private bool _isDarkChecked;

        public bool IsDarkChecked
        {
            get { return _isDarkChecked; }
            set
            {
                if (SetProperty(ref _isDarkChecked, value))
                    _othersConfig.Theme = value ? Theme.SpecifiedColor.Dark : Theme.SpecifiedColor.Light;
            }
        }

        #endregion

        #region IsLightChecked

        private bool _isLightChecked;

        public bool IsLightChecked
        {
            get { return _isLightChecked; }
            set
            {
                // いらないだろうけど。
                if (SetProperty(ref _isLightChecked, value))
                    _othersConfig.Theme = value ? Theme.SpecifiedColor.Light : Theme.SpecifiedColor.Dark;
            }
        }

        #endregion
    }
}