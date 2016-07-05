using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Threading;

using DesktopToast;

using Hardcodet.Wpf.TaskbarNotification;

using Norma.Eta;
using Norma.Eta.Properties;
using Norma.Gamma.Models;
using Norma.Ipsilon.Notifications;
using Norma.Ipsilon.Views;

using NotificationsExtensions;
using NotificationsExtensions.Toasts;

namespace Norma.Ipsilon.Models
{
    internal static class NotificationManager
    {
        public static async Task<string> ShowNotification(string title, string body, Slot slot)
        {
            var result = "";
            if (NormaConstants.IsSupportedToast)
            {
                var toastXml = ComposeInteractiveToast(title, body, slot);
                var toastRequest = new ToastRequest
                {
                    ToastXml = toastXml,
                    ShortcutFileName = NormaConstants.IpsilonLinkName,
#if DEBUG
                    ShortcutTargetFilePath = Assembly.GetExecutingAssembly().Location,
#else
                    ShortcutTargetFilePath = NormaConstants.IpsilonExecutableFile,
#endif
                    AppId = NormaConstants.IpsilonAppId,
                    ToastLogoFilePath = $"file:///{Path.GetFullPath("Resources/128.png")}"
                };
                if (NormaConstants.IsSupportedNewToast)
                    toastRequest.ActivatorId = typeof(NotificationActivator).GUID;
                var rs = await ToastManager.ShowAsync(toastRequest);
                result = rs.ToString();
            }
            else
                Dispatcher.CurrentDispatcher.Invoke(() =>
                {
                    //
                    Shell.TaskbarIcon.ShowBalloonTip(title, body, BalloonIcon.Info);
                });

            return result;
        }

        private static string ComposeInteractiveToast(string title, string body, Slot slot)
        {
            // なぜか、 image タグが反映されない。
            var toastVisual = new ToastVisual
            {
                BindingGeneric = new ToastBindingGeneric
                {
                    Children =
                    {
                        new AdaptiveText {Text = title},
                        new AdaptiveText {Text = body}
                    }
                }
            };
            var toastAction = new ToastActionsCustom
            {
                Buttons =
                {
                    new ToastButton(Resources.View, $"action=View&channelId={slot?.ChannelId}")
                    {
                        ActivationType = ToastActivationType.Background
                    },
                    new ToastButton(Resources.Ignore, "action=Ignored")
                }
            };
            var toastContent = new ToastContent
            {
                Visual = toastVisual,
                Actions = toastAction,
                Duration = ToastDuration.Long
            };

            Debug.WriteLine(toastContent.GetContent());
            return toastContent.GetContent();
        }
    }
}