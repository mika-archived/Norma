using System.Windows;

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

            AppInitializer.PreInitialize(this);

            var bootstrap = new Bootstrapper();
            bootstrap.Run();
        }

        #endregion
    }
}