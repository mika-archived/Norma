using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Web;

using DesktopToast;

using Norma.Eta;
using Norma.Eta.Models;
using Norma.Eta.Models.Operations;
using Norma.Ipsilon.Models;
using Norma.Ipsilon.Notifications;

namespace Norma.Ipsilon
{
    internal static class AppInitializer
    {
        public static AbemaApiHost AbemaApiHost { get; private set; }
        public static Configuration Configuration { get; private set; }
        public static Timetable Timetable { get; private set; }
        public static ConnectOps ConnectOps { get; private set; }

        public static void PreInitialize()
        {
            Configuration = new Configuration();
            AbemaApiHost = new AbemaApiHost(Configuration);
            Timetable = new Timetable(AbemaApiHost);
            ConnectOps = new ConnectOps();
        }

        public static void Initialize()
        {
            AbemaApiHost.Initialize();
            Timetable.Sync();
        }

        public static void PostInitialize()
        {
            if (!NormaConstants.IsSupportedNewToast)
                return;

            // For Action Center of Windows 10
            NotificationActivatorBase.RegisterComType(typeof(NotificationActivator), OnActivated);
            NotificationHelper.RegisterComServer(typeof(NotificationActivator), Assembly.GetExecutingAssembly().Location);
        }

        private static void OnActivated(string s, Dictionary<string, string> dictionary)
        {
            var r = NotificationResult.Activated;
            if ((s?.StartsWith("action=")).GetValueOrDefault())
            {
                var result = s?.Substring("action=".Length);
                switch (result)
                {
                    case "TimedOut":
                        r = NotificationResult.TimedOut;
                        break;

                    case "Ignored":
                        r = NotificationResult.Canceled;
                        break;

                    // View&channel=???
                    default:
                        r = NotificationResult.Activated;
                        break;
                }
            }
            if (r != NotificationResult.Activated)
                return;
            // Launch application or Change channel.
            LaunchApplicationIfNotLaunched();
            ChanngeChannel(s);
        }

        private static void LaunchApplicationIfNotLaunched()
        {
            var processes = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(NormaConstants.MainFileName));
            if (processes.Length == 0)
#if !DEBUG
                if (File.Exists(NormaConstants.MainExecutableFile))
                    Process.Start(NormaConstants.MainExecutableFile);
#endif
                Debug.WriteLine("DebugMode=true");
        }

        private static void ChanngeChannel(string queryParameters)
        {
            var parameters = HttpUtility.ParseQueryString(queryParameters);
            if (string.IsNullOrWhiteSpace(parameters["channelId"]))
                return;
            var channel = parameters["channelId"];

            ConnectOps.Save(new ChangeChannelOp(channel));
        }
    }
}