using System.Collections.Generic;
using System.Reflection;

using DesktopToast;

using Norma.Eta;
using Norma.Eta.Models;
using Norma.Ipsilon.Models;
using Norma.Ipsilon.Notifications;

namespace Norma.Ipsilon
{
    internal static class AppInitializer
    {
        public static AbemaApiHost AbemaApiHost { get; private set; }
        public static Configuration Configuration { get; private set; }
        public static Timetable Timetable { get; private set; }

        public static void PreInitialize()
        {
            Configuration = new Configuration();
            AbemaApiHost = new AbemaApiHost(Configuration);
            Timetable = new Timetable(AbemaApiHost);
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
                var result = s.Substring("action=".Length);
                switch (result)
                {
                    case "TimedOut":
                        r = NotificationResult.TimedOut;
                        break;

                    default:
                        r = NotificationResult.Canceled;
                        break;
                }
            }
            if (r != NotificationResult.Activated)
                return;
            // Launch application or Change channel.
        }
    }
}