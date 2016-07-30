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

        public JavaScriptHost(IWpfWebBrowser wpfWebBrowser, Configuration configuration)
        {
            _wpfWebBrowser = wpfWebBrowser;
            _configuration = configuration;
            _wpfWebBrowser.ConsoleMessage += (sender, e) => Debug.WriteLine("[Chromium]" + e.Message);
            _wpfWebBrowser.FrameLoadEnd += (sender, e) =>
            {
                if (!e.Url.StartsWith("https://abema.tv/now-on-air/"))
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
            CheckShouldExecuteJavaScript();
            if (_configuration.Root.Browser.DisableChangeChannelByMouseWheel)
                DisableChangeChannelByMouseScroll();
            DisableContextMenu();
            InjectCustomCss();
            ExecutedScripts();
        }

        private void CheckShouldExecuteJavaScript()
        {
            const string jsCode = @"
if (!('shouldExecute' in this)) {
  console.log('Initialize');
  var shouldExecute = false;
}
if('isLoadedByCef' in this) {
  console.log('JavaScript already is injected.');
} else {
  shouldExecute = true;
  isLoadedByCef = 1;
  console.log('JavaScript injection enabled.');
}
";
            WrapExecuteScriptAsync(jsCode);
        }

        private void ExecutedScripts()
        {
            const string jsCode = @"
shouldExecute = false
";
            WrapExecuteScriptAsync(jsCode);
        }

        private void DisableChangeChannelByMouseScroll()
        {
            const string jsCode = @"
if (shouldExecute) {
  window.addEventListener('mousewheel', function(e) {
    e.stopImmediatePropagation();
  }, true);
}
";
            StatusInfo.Instance.Text = Resources.DisableChangeChannelByMouseWheel;
            WrapExecuteScriptAsync(jsCode);
        }

        private void DisableContextMenu()
        {
            const string jsCode = @"
if (shouldExecute) {
  window.addEventListener('contextmenu', function(e) {
    e.preventDefault();
  }, true);
}
";
            StatusInfo.Instance.Text = Resources.DisableContextMenu;
            WrapExecuteScriptAsync(jsCode);
        }

        private void InjectCustomCss()
        {
            var css = _configuration.Root.Browser.CustomCss.Replace(Environment.NewLine, "").Replace("'", "\\'");
            string jsCode =
                $@"
if (shouldExecute) {{
  function injectNormaCustomCss() {{
    var style = document.createElement('style');
    style.media = 'screen';
    style.type = 'text/css';

    var rule = document.createTextNode('{css}');
    if (style.styleSheet) {{
      style.styleSheet = rule.nodeValue;
    }} else {{
      style.appendChild(rule);
    }}
    var head = document.getElementsByTagName('head')[0];
    head.appendChild(style);
    console.log('Injected custom css');
  }};
  setTimeout(injectNormaCustomCss, 500);
}}";
            StatusInfo.Instance.Text = Resources.InjectCss;
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