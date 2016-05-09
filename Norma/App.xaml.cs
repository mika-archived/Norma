using System.Windows;

using Norma.Models;

namespace Norma
{
    /// <summary>
    ///     App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        #region Overrides of Application

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            CefSetting.Init();
            Configuration.Instance.Load();
            await AbemaApiHost.Instance.Initialize();
            await Timetable.Instance.Sync();

            var bootstrap = new Bootstrapper();
            bootstrap.Run();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            Configuration.Instance.Save();
            base.OnExit(e);
        }

        #endregion
    }
}