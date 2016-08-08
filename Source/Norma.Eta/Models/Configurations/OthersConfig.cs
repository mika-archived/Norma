using MetroRadiance.UI;

using Newtonsoft.Json;

using Norma.Eta.Models.Enums;

using Prism.Mvvm;

using ColorTheme = MetroRadiance.UI.Theme;

namespace Norma.Eta.Models.Configurations
{
    public class OthersConfig : BindableBase
    {
        [JsonProperty]
        public bool IsEnabledExperimentalFeatures { get; set; }

        public OthersConfig()
        {
            Theme = UITheme.Dark;
            IsEnabledExperimentalFeatures = false;
        }

        private ColorTheme GetThemeFromSpecifiedColor(UITheme color)
            => color == UITheme.Dark ? ColorTheme.Dark : (color == UITheme.Light ? ColorTheme.Light : ColorTheme.Windows);

        #region Theme

        private UITheme _theme;

        public UITheme Theme
        {
            get { return _theme; }
            set
            {
                if (!SetProperty(ref _theme, value))
                    return;
                ThemeService.Current.ChangeTheme(GetThemeFromSpecifiedColor(value));
                ThemeService.Current.ChangeAccent(Theme == UITheme.Windows ? Accent.Windows : Accent.Blue);
            }
        }

        #endregion
    }
}