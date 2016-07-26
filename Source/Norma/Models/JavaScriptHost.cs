using System;
using System.Diagnostics;

using CefSharp;
using CefSharp.Wpf;

using Norma.Eta.Models;
using Norma.Eta.Properties;

using Prism.Mvvm;

namespace Norma.Models
{
    // References: https://github.com/nakayuki805/AbemaTVChromeExtension
    // Not support features are:
    //  * CM related features.
    //  * Comment related features.
    internal class JavaScriptHost : BindableBase, IDisposable
    {
        private readonly Configuration _configuration;
        private readonly IWpfWebBrowser _wpfWebBrowser;
        // private IDisposable _disposable;

        public string Address { get; set; }

        public JavaScriptHost(IWpfWebBrowser wpfWebBrowser, Configuration configuration)
        {
            _wpfWebBrowser = wpfWebBrowser;
            _configuration = configuration;
            Address = "";
            _wpfWebBrowser.ConsoleMessage += (sender, e) => Debug.WriteLine("[Chromium]" + e.Message);
            _wpfWebBrowser.FrameLoadEnd += (sender, e) =>
            {
                if (!Address.StartsWith("https://abema.tv/now-on-air/"))
                    return;
                Run();
            };
        }

        #region Implementation of IDisposable

        public void Dispose()
        {
            // _disposable?.Dispose();
        }

        #endregion

        private void Run()
        {
            if (_configuration.Root.Browser.DisableChangeChannelByMouseWheel)
                DisableChangeChannelByMouseScroll();
            DisableContextMenu();
            if (_configuration.Root.Browser.HiddenHeaderControls)
                HideTvContainerHeader();
            if (_configuration.Root.Browser.HiddenFooterControls)
                HideTvContainerFooter();
            if (_configuration.Root.Browser.HiddenSideControls)
                HideTvContainerSide();
            HideTwitterContainer();
        }

        private void DisableChangeChannelByMouseScroll()
        {
            const string jsCode = @"
window.addEventListener('mousewheel', function(e) {
  e.stopImmediatePropagation();
}, true);
";
            StatusInfo.Instance.Text = Resources.DisableChangeChannelByMouseWheel;
            WrapExecuteScriptAsync(jsCode);
        }

        private void DisableContextMenu()
        {
            const string jsCode = @"
window.addEventListener('contextmenu', function(e) {
  e.preventDefault();
}, true);
";
            StatusInfo.Instance.Text = Resources.DisableContextMenu;
            WrapExecuteScriptAsync(jsCode);
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
            StatusInfo.Instance.Text = Resources.HiddenHeaderControls;
            WrapExecuteScriptAsync(jsCode);
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
            StatusInfo.Instance.Text = Resources.HiddenFooterControls;
            WrapExecuteScriptAsync(jsCode);
        }

        private void HideTvContainerSide()
        {
            const string jsCode = @"
function cs_HideTvContainerSide() {
  var appContainerSide = window.document.querySelector('[class^=""TVContainer__side___""]');
  if (appContainerSide == null) {
    return;
  }
  appContainerSide.style.display = 'none';
};
setTimeout(cs_HideTvContainerSide, 500);
";
            StatusInfo.Instance.Text = Resources.HiddenSideControls;
            WrapExecuteScriptAsync(jsCode);
        }

        private void HideTwitterContainer()
        {
            const string jsCode = @"
function cs_HideTvContainerSide() {
  var appContainerSide = window.document.querySelector('[class^=""styles__container___""]');
  if (appContainerSide == null) {
    return;
  }
  appContainerSide.style.display = 'none';
};
setTimeout(cs_HideTvContainerSide, 500);
";
            // StatusInfo.Instance.Text = Resources.HiddenSideControls;
            WrapExecuteScriptAsync(jsCode);
        }

        #region Wrap IWpfWebBrowser Js Executor

        private void WrapExecuteScriptAsync(string jsCode)
        {
            try
            {
                _wpfWebBrowser.ExecuteScriptAsync(jsCode);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        #endregion
    }
}