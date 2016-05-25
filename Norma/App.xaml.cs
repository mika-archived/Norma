using System.Windows;

using MetroRadiance.UI;

using Norma.Models;

namespace Norma
{
    /// <summary>
    ///     App.xaml の相互作用ロジック
    /// </summary>
    // ReSharper disable once RedundantExtendsListEntry
    public partial class App : Application
    {
        #region Overrides of Application

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            CefSetting.Init();
            ThemeService.Current.Register(this, Theme.Dark, Accent.Blue);

            var bootstrap = new Bootstrapper();
            bootstrap.Run();
        }

        #endregion
    }
}