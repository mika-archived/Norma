using MetroRadiance.UI;

using Newtonsoft.Json;

using Prism.Mvvm;

using ColorTheme = MetroRadiance.UI.Theme;
using ThemeColor = MetroRadiance.UI.Theme.SpecifiedColor;

namespace Norma.Eta.Models.Configurations
{
    public class OthersConfig : BindableBase
    {
        [JsonProperty]
        public bool IsEnabledExperimentalFeatures { get; set; }

        public OthersConfig()
        {
            Theme = ThemeColor.Dark;
            IsEnabledExperimentalFeatures = false;
        }

        private ColorTheme GetThemeFromSpecifiedColor(ThemeColor color)
            => color == ThemeColor.Dark ? ColorTheme.Dark : ColorTheme.Light;

        #region Theme

        private ThemeColor _theme;

        public ThemeColor Theme
        {
            get { return _theme; }
            set
            {
                if (SetProperty(ref _theme, value))
                    ThemeService.Current.ChangeTheme(GetThemeFromSpecifiedColor(value));
            }
        }

        #endregion
    }
}