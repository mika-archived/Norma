using System.Windows;

using MetroRadiance.UI;

namespace Norma.Iota
{
    /// <summary>
    ///     App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        #region Overrides of Application

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ThemeService.Current.Register(this, Theme.Dark, Accent.Blue);

            var bootstrap = new Bootstrapper();
            bootstrap.Run();
        }

        #endregion
    }
}