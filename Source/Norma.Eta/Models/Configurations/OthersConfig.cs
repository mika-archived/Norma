using MetroRadiance.UI;

using Prism.Mvvm;

using ColorTheme = MetroRadiance.UI.Theme;
using ThemeColor = MetroRadiance.UI.Theme.SpecifiedColor;

namespace Norma.Eta.Models.Configurations
{
    public class OthersConfig : BindableBase
    {
        public OthersConfig()
        {
            Theme = ThemeColor.Dark;
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