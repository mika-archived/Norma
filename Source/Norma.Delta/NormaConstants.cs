using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using EnvSpecialFolder = System.Environment.SpecialFolder;

namespace Norma.Delta
{
    internal static class NormaConstants
    {
        private static readonly string AppDirectory =
            Path.Combine(Environment.GetFolderPath(EnvSpecialFolder.ApplicationData), "kokoiroworks.com", "Norma");

        private static string DatabaseFile => Path.Combine(AppDirectory, "application.db");

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