using System;
using System.IO;

using CefSharp;

using EnvSpecialFolder = System.Environment.SpecialFolder;

namespace Norma.Models
{
    // CefSharp Settings
    internal static class CefSetting
    {
        private static readonly string AppDir =
            Path.Combine(Environment.GetFolderPath(EnvSpecialFolder.ApplicationData), "kokoiroworks.com", "Norma");

        internal static void Init()
        {
            var settings = new CefSettings
            {
                CachePath = Path.Combine(AppDir, "cache"),
                MultiThreadedMessageLoop = true,
                WindowlessRenderingEnabled = true
            };
            settings.EnableInternalPdfViewerOffScreen();
            settings.CefCommandLineArgs.Add("disable-gpu", "1");

            Cef.OnContextInitialized = () =>
            {
                var cookieManager = Cef.GetGlobalCookieManager();
                cookieManager.SetStoragePath(Path.Combine(AppDir, "cookies"), true);
            };

            if (!Cef.Initialize(settings, true, false))
                throw new Exception("Unable to Initialize Chromium Embedded Framework");
        }
    }
}