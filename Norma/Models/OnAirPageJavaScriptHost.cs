using System;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Threading.Tasks;

using CefSharp;
using CefSharp.Wpf;

using Prism.Mvvm;

namespace Norma.Models
{
    // References: https://github.com/nakayuki805/AbemaTVChromeExtension
    // Not support features are:
    //  * CM related features.
    //  * Comment related features.
    internal class OnAirPageJavaScriptHost : BindableBase, IDisposable
    {
        private readonly IWpfWebBrowser _wpfWebBrowser;
        private IDisposable _disposable;

        public string Address { get; set; }

        public OnAirPageJavaScriptHost(IWpfWebBrowser wpfWebBrowser)
        {
            _wpfWebBrowser = wpfWebBrowser;
            Address = "";
            _wpfWebBrowser.ConsoleMessage += (sender, e) => Debug.WriteLine("[Chromium]" + e.Message);
            _wpfWebBrowser.FrameLoadStart += (sender, e) => _disposable?.Dispose();
            _wpfWebBrowser.FrameLoadEnd += (sender, e) =>
            {
                if (!Address.StartsWith("https://abema.tv/now-on-air/"))
                    return;
                Run();
                Observable.Return(1).Delay(TimeSpan.FromSeconds(1)).Subscribe(w => RunLater());
            };
        }

        #region Implementation of IDisposable

        public void Dispose()
        {
            _disposable.Dispose();
        }

        #endregion

        // TODO: Toggle enable/disable features by settings.
        private void Run()
        {
            DisableChangeChannelByMouseScroll();
            HideTvContainerHeader();
            HideTvContainerFooter();
            HideTvContainerSide();
        }

        private void RunLater()
        {
            // 1秒遅らせている都合上、 null になることがある
            if (_wpfWebBrowser == null)
                return;

            _disposable = Observable.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(1)).Subscribe(w => GetTitleInfo());
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
            _wpfWebBrowser.ExecuteScriptAsync(jsCode);
        }

        private void GetTitleInfo()
        {
            const string jsCode = @"
(function () {
  var appContainerHeading = window.document.querySelector('[class^=""style__heading2___""]');
  if (appContainerHeading == null) {
    return 'null';
  }
  return appContainerHeading.innerHTML;
})();
";
            var task = _wpfWebBrowser.EvaluateScriptAsync(jsCode, null);
            task.ContinueWith(w =>
            {
                if (w.IsFaulted)
                    return;
                var response = task.Result;
                if (!response.Success || response.Result.ToString() == "null")
                    Title = "(CM) - Norma";
                else
                    Title = $"{response.Result.ToString()} - Norma";
            }, TaskScheduler.Default);
        }

        #region Title

        private string _title;

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        #endregion
    }
}