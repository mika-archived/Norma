using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

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

        public static string CrashReportsDir => Path.Combine(AppDirectory, "crashreports");

        public static string ConfigurationFile => Path.Combine(AppDirectory, "config.json");

        public static string TimetableCacheFile => Path.Combine(AppDirectory, "timetable_cache.json");

        public static string ReserveProgramListFile => Path.Combine(AppDirectory, "reserve_programs.json");

        public static string ReserveProgramLockFile => Path.Combine(AppDirectory, "reserve_programs.lock");

        public static string OpsFile => Path.Combine(AppDirectory, "ops.json");

        public static string MainFileName => "Norma.exe";

        public static string MainExecutableFile => Path.Combine(CurrentDirectory, MainFileName);

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

        private static string DatabaseFile => Path.Combine(AppDirectory, "reserve_programs.db");

        public static string DatabaseConnectionString => $"Data Source={DatabaseFile};{DatabaseOptions()}";

        public static string DatabaseProvider => "System.Data.SQLite";

        private static string DatabaseOptions()
        {
            var dictionary = new Dictionary<string, string>
            {
                {"Default IsolationLevel", "Serializable"},
                {"SyncMode", "Off"},
                {"JournalMode", "Wal"}
            };
            var sb = new StringBuilder();
            foreach (var kvp in dictionary)
                sb.Append($"{kvp.Key}={kvp.Value};");
            return sb.ToString();
        }
    }
}