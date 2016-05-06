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

        public string Address { get; set; }

        public OnAirPageJavaScriptHost(IWpfWebBrowser wpfWebBrowser)
        {
            _wpfWebBrowser = wpfWebBrowser;
            Address = "";
            _wpfWebBrowser.ConsoleMessage += (sender, e) => Debug.WriteLine("[Chromium]" + e.Message);
            _wpfWebBrowser.FrameLoadEnd += (sender, e) =>
            {
                if (Address.StartsWith("https://abema.tv/now-on-air/"))
                    Run();
            };
        }

        // TODO: Toggle enable/disable features by settings.
        private void Run()
        {
            DisableChangeChannelByMouseScroll();
            HideTvContainerHeader();
            HideTvContainerFooter();
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

        private void HideTvContainerHeader()
        {
            const string jsCode = @"
function cs_HideTvContainerHeader() {
  var appContainerHeader = window.document.querySelector('[class^=""AppContainer__header-container___""]');
  if (appContainerHeader == null) {
    return;
  }
  appContainerHeader.style.display = 'none';
};
setTimeout(cs_HideTvContainerHeader, 500);
";
            _wpfWebBrowser.ExecuteScriptAsync(jsCode);
        }

        private void HideTvContainerFooter()
        {
            const string jsCode = @"
function cs_HideTvContainerFooter() {
  var appContainerFooter = window.document.querySelector('[class^=""TVContainer__footer-container___""]');
  if (appContainerFooter == null) {
    return;
  }
  appContainerFooter.style.display = 'none';
};
setTimeout(cs_HideTvContainerFooter, 500);
";
            _wpfWebBrowser.ExecuteScriptAsync(jsCode);
        }
    }
}