using System.Diagnostics;
using System.IO;
using System.Windows;

using MetroRadiance.UI;

using Microsoft.Practices.ServiceLocation;

using Norma.Delta.Services;
using Norma.Eta;
using Norma.Eta.Services;
using Norma.Models.Browser;
using Norma.Views;

namespace Norma
{
    /// <summary>
    ///     アプリケーションの初期化時に呼ばれます。
    /// </summary>
    internal static class AppInitializer
    {
        private static StartupScreen _startupScreen;
        private static StatusService _statusService;

        /// <summary>
        ///     PreInitialize is called by Application host.
        /// </summary>
        /// <param name="application"></param>
        public static void PreInitialize(Application application)
        {
            _startupScreen = new StartupScreen();
            _startupScreen.Show();

            if (!Directory.Exists(NormaConstants.CrashReportsDir))
                Directory.CreateDirectory(NormaConstants.CrashReportsDir);

            CefSetting.Init();
            ThemeService.Current.Register(application, Theme.Dark, Accent.Blue);
        }

        /// <summary>
        ///     Initialize is called by Bootstrapper.
        /// </summary>
        public static void Initialize()
        {
            _statusService = ServiceLocator.Current.GetInstance<StatusService>();

            _statusService.UpdateStatus("データベースの更新をしています...");
            var databaseService = ServiceLocator.Current.GetInstance<DatabaseService>();
            databaseService.Initialize();
            databaseService.Migration();
        }

        /// <summary>
        ///     PostInitialize is called by Bootstrapper.
        /// </summary>
        public static void PostInitialize()
        {
            var processes = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(NormaConstants.IpsilonFileName));
            if (processes.Length == 0)
                if (File.Exists(NormaConstants.IpsilonExecutableFile))
                    Process.Start(NormaConstants.IpsilonExecutableFile);

            _startupScreen.Hide();
            _startupScreen.Close();
            _startupScreen = null;
        }
    }
}