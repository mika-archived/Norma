using System;

using CefSharp;

using EnvSpecialFolder = System.Environment.SpecialFolder;

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
            settings.EnableInternalPdfViewerOffScreen();
            settings.CefCommandLineArgs.Add("disable-gpu", "1");

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