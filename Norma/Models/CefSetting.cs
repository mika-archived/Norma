using System;

using CefSharp;

using Norma.Eta;

namespace Norma.Models
{
    // CefSharp Settings
    internal static class CefSetting
    {
        internal static void Init()
        {
            var settings = new CefSettings
            {
                CachePath = NormaConstants.CefCacheDir,
                MultiThreadedMessageLoop = true,
                WindowlessRenderingEnabled = true
            };
            settings.CefCommandLineArgs.Add("disable-extensions", "1");
            settings.CefCommandLineArgs.Add("disable-pdf-extension", "1");
            settings.CefCommandLineArgs.Add("disable-surfaces", "1");
            settings.CefCommandLineArgs.Add("disable-gpu", "1");
            settings.CefCommandLineArgs.Add("disable-gpu-compositing", "1");
            settings.CefCommandLineArgs.Add("enable-begin-frame-scheduling", "1");

            Cef.OnContextInitialized = () =>
            {
                var cookieManager = Cef.GetGlobalCookieManager();
                cookieManager.SetStoragePath(NormaConstants.CefCookiesDir, true);
            };

            if (!Cef.Initialize(settings, true, false))
                throw new Exception("Unable to Initialize Chromium Embedded Framework");
        }
    }
}