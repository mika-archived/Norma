using System.Diagnostics;
using System.IO;
using System.Windows;

using MetroRadiance.UI;

using Norma.Eta;
using Norma.Eta.Models;
using Norma.Views;

namespace Norma.Models
{
    /// <summary>
    ///     アプリケーションの初期化時に呼ばれます。
    /// </summary>
    internal class AppInitializer
    {
        private static StartupScreen _startupScreen;
        public static AbemaApiHost AbemaApiHost { get; private set; }
        public static AbemaState AbemaState { get; private set; }
        public static Configuration Configuration { get; private set; }
        public static Timetable Timetable { get; private set; }

        /// <summary>
        ///     PreInitialize is called by Application host.
        /// </summary>
        /// <param name="application"></param>
        public static void PreInitialize(Application application)
        {
            _startupScreen = new StartupScreen();
            _startupScreen.Show();

            CefSetting.Init();
            ThemeService.Current.Register(application, Theme.Dark, Accent.Blue);

            Configuration = new Configuration();
            AbemaApiHost = new AbemaApiHost(Configuration);
            Timetable = new Timetable(AbemaApiHost);
            AbemaState = new AbemaState(Configuration, Timetable);
        }

        /// <summary>
        ///     Initialize is called by Bootstrapper.
        /// </summary>
        public static void Initialize()
        {
            AbemaApiHost.Initialize();
            Timetable.Sync();
            AbemaState.Start();
        }

        /// <summary>
        ///     PostInitialize is called by Bootstrapper.
        /// </summary>
        public static void PostInitialize()
        {
            var processes = Process.GetProcessesByName(NormaConstants.IpsilonFileName);
            if (processes.Length == 0)
                if (File.Exists(NormaConstants.IpsilonExecutableFile))
                    Process.Start(NormaConstants.IpsilonExecutableFile);

            _startupScreen.Hide();
            _startupScreen.Close();
            _startupScreen = null;
        }
    }
}