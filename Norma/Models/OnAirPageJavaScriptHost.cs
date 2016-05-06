using System.Diagnostics;

using CefSharp;
using CefSharp.Wpf;

namespace Norma.Models
{
    // References: https://github.com/nakayuki805/AbemaTVChromeExtension
    // Not support features are:
    //  * CM related features.
    //  * Comment related features.
    internal class OnAirPageJavaScriptHost
    {
        private readonly IWpfWebBrowser _wpfWebBrowser;

        public OnAirPageJavaScriptHost(IWpfWebBrowser wpfWebBrowser)
        {
            _wpfWebBrowser = wpfWebBrowser;
            _wpfWebBrowser.ConsoleMessage += (sender, e) => Debug.WriteLine("[Chromium]" + e.Message);
            _wpfWebBrowser.FrameLoadEnd += (sender, e) => Run();
        }

        // TODO: Toggle enable/disable features by settings.
        private void Run()
        {
            DisableChangeChannelByMouseScroll();
        }

        private void DisableChangeChannelByMouseScroll()
        {
            const string jsCode = @"
window.addEventListener('mousewheel', function(e) {
  e.stopImmediatePropagation();
}, true);
";
            _wpfWebBrowser.ExecuteScriptAsync(jsCode);
        }
    }
}