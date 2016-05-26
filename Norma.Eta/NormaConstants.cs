using System;
using System.IO;

using EnvSpecialFolder = System.Environment.SpecialFolder;

namespace Norma.Eta
{
    public class NormaConstants
    {
        private static readonly string AppDirectory =
            Path.Combine(Environment.GetFolderPath(EnvSpecialFolder.ApplicationData), "kokoiroworks.com", "Norma");

        private static readonly string CurrentDirectory = Directory.GetCurrentDirectory();

        public static string CefCacheDir => Path.Combine(AppDirectory, "cache");

        public static string CefCookiesDir => Path.Combine(AppDirectory, "cookies");

        public static string ConfigurationFile => Path.Combine(AppDirectory, "config.json");

        public static string TimetableCacheFile => Path.Combine(AppDirectory, "timetable_cache.json");

        public static string IotaExecutableFile => Path.Combine(CurrentDirectory, "Norma.Iota.exe");

        public static string IpsilonExecutableFile => Path.Combine(CurrentDirectory, "Norma.Ipsilon.exe");
    }
}