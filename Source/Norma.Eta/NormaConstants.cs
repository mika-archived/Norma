using System;
using System.IO;

using EnvSpecialFolder = System.Environment.SpecialFolder;

namespace Norma.Eta
{
    public static class NormaConstants
    {
        private static readonly string AppDirectory =
            Path.Combine(Environment.GetFolderPath(EnvSpecialFolder.ApplicationData), "kokoiroworks.com", "Norma");

        private static readonly string CurrentDirectory = Directory.GetCurrentDirectory();

        public static string CefCacheDir => Path.Combine(AppDirectory, "cache");

        public static string CefCookiesDir => Path.Combine(AppDirectory, "cookies");

        public static string ConfigurationFile => Path.Combine(AppDirectory, "config.json");

        public static string TimetableCacheFile => Path.Combine(AppDirectory, "timetable_cache.json");

        public static string ReserveProgramListFile => Path.Combine(AppDirectory, "reserve_programs.json");

        public static string ReserveProgramLockFile => Path.Combine(AppDirectory, "reserve_programs.lock");

        public static string OpsFile => Path.Combine(AppDirectory, "ops.json");

        public static string IotaFileName => "Norma.Iota.exe";

        public static string IotaExecutableFile => Path.Combine(CurrentDirectory, IotaFileName);

        public static string IpsilonFileName => "Norma.Ipsilon.exe";

        public static string IpsilonLinkName => "Norma.Ipsilon.lnk";

        public static string IpsilonAppId => "Norma.Ipsilon";

        public static string IpsilonExecutableFile => Path.Combine(CurrentDirectory, IpsilonFileName);

        // Windows 10
        public static bool IsSupportedNewToast
            => Environment.OSVersion.Version.Major >= 10;

        // Windows 8.0 ~
        public static bool IsSupportedToast
            => (Environment.OSVersion.Version.Major == 6 && Environment.OSVersion.Version.Minor >= 2) ||
               IsSupportedNewToast;

        // Windows 8, Windows 7, Windows Vista
        // 諦めてくれ
        public static bool NotSupportedVersion
            => Environment.OSVersion.Version.Major == 6 && Environment.OSVersion.Version.Minor < 3;
    }
}