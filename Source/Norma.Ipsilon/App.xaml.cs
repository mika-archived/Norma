using System;
using System.Threading;
using System.Windows;

using DesktopToast;

using Norma.Eta;
using Norma.Ipsilon.Views;

namespace Norma.Ipsilon
{
    /// <summary>
    ///     App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        private Mutex _mutex;

        #region Overrides of Application

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            AppInitializer.PreInitialize();

            _mutex = new Mutex(false, NormaConstants.IpsilonAppId);
            if (!_mutex.WaitOne(0, false))
            {
                _mutex.Close();
                Shutdown();
            }

            var bootstrapper = new Bootstrapper();
            bootstrapper.Run();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            if (NormaConstants.IsSupportedNewToast)
                NotificationActivatorBase.UnregisterComType();
            (Shell.TaskbarIcon.DataContext as IDisposable)?.Dispose();
            Shell.TaskbarIcon.Dispose();

            _mutex?.ReleaseMutex();
            _mutex?.Close();
            base.OnExit(e);
        }

        #endregion
    }
}