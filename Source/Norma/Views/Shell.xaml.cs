using System;
using System.Configuration;
using System.Windows;

using MetroRadiance.UI.Controls;

using Norma.Eta.Controls;

namespace Norma.Views
{
    /// <summary>
    ///     Shell.xaml の相互作用ロジック
    /// </summary>
    public partial class Shell : MetroWindow
    {
        public Shell()
        {
            InitializeComponent();
        }

        #region Overrides of MetroWindow

        protected override void OnSourceInitialized(EventArgs e)
        {
            WindowSettings = new WindowSettingsExt(this);
            var value = ((WindowSettingsExt) WindowSettings).UpdateRequired;
            if (value.HasValue && value.Value)
                ((ApplicationSettingsBase) WindowSettings).Upgrade();
            ((WindowSettingsExt) WindowSettings).UpdateRequired = false;
            ((WindowSettingsExt) WindowSettings).Save();

            base.OnSourceInitialized(e);
        }

        #endregion

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.Shutdown(); // なぜか終了しない。
        }
    }
}