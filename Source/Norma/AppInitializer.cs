using System.Diagnostics;
using System.IO;
using System.Windows;

using MetroRadiance.UI;

using Microsoft.Practices.ServiceLocation;

using Norma.Delta.Services;
using Norma.Eta;
using Norma.Eta.Models;
using Norma.Eta.Services;
using Norma.Models;
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

        /// <summary>
        ///     PreInitialize is called by Application host.
        /// </summary>
        /// <param name="application"></param>
        public static void PreInitialize(Application application)
        {
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
            _startupScreen = new StartupScreen();
            _startupScreen.Show();

            var updater = new Updater(ServiceLocator.Current.GetInstance<Configuration>());
            if (updater.IsPublishedUpdate())
                updater.Update();

            var databaseService = ServiceLocator.Current.GetInstance<DatabaseService>();
            using (var connection = databaseService.Connect())
            {
                connection.Initialize();
                connection.Migration();
            }

            var abemaApiClient = ServiceLocator.Current.GetInstance<AbemaApiClient>();
            abemaApiClient.Initialize();

            var timetableService = ServiceLocator.Current.GetInstance<TimetableService>();
            timetableService.Initialize();
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