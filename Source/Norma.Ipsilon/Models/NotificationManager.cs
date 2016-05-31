using System.Threading.Tasks;

using DesktopToast;

using Hardcodet.Wpf.TaskbarNotification;

using Norma.Eta;
using Norma.Ipsilon.Notifications;
using Norma.Ipsilon.Views;

namespace Norma.Ipsilon.Models
{
    internal static class NotificationManager
    {
        // TODO: AppLogoOverride で、アイコンを番組のサムネイルにする。
        public static async Task<string> ShowNotification(string title, string body)
        {
            var result = "";
            if (NormaConstants.IsSupportedToast)
            {
                var toastRequest = new ToastRequest
                {
                    ToastTitle = title,
                    ToastBody = body,
                    ShortcutFileName = NormaConstants.IpsilonLinkName,
                    ShortcutTargetFilePath = NormaConstants.IpsilonExecutableFile,
                    AppId = NormaConstants.IpsilonAppId
                };
                if (NormaConstants.IsSupportedNewToast)
                    toastRequest.ActivatorId = typeof(NotificationActivator).GUID;
                var rs = await ToastManager.ShowAsync(toastRequest);
                result = rs.ToString();
            }
            else
                Shell.TaskbarIcon.ShowBalloonTip(title, body, BalloonIcon.Info);

            return result;
        }
    }
}