using System;
using System.IO;

using EnvSpecialFolder = System.Environment.SpecialFolder;

namespace Norma
{
    internal static class NormaConstants
    {
        private static readonly string AppDirectory =
            Path.Combine(Environment.GetFolderPath(EnvSpecialFolder.ApplicationData), "kokoiroworks.com", "Norma");

        public static string CefCacheDir => Path.Combine(AppDirectory, "cache");

        public static string CefCookiesDir => Path.Combine(AppDirectory, "cookies");

        public static string ConfigurationFile => Path.Combine(AppDirectory, "config.json");
    }
}