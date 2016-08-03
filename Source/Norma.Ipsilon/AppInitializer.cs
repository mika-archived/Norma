using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Web;

using DesktopToast;

using Microsoft.Practices.ServiceLocation;

using Norma.Delta.Services;
using Norma.Eta;
using Norma.Eta.Models;
using Norma.Eta.Models.Operations;
using Norma.Eta.Services;
using Norma.Ipsilon.Models;
using Norma.Ipsilon.Notifications;

namespace Norma.Ipsilon
{
    internal static class AppInitializer
    {
        public static void PreInitialize()
        {
            //
        }

        public static void Initialize()
        {
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

            var connector = ServiceLocator.Current.GetInstance<ConnectOps>();
            connector.Save(new ChangeChannelOp(channel));
        }
    }
}