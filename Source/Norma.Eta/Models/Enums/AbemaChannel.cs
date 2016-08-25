using System.Diagnostics;

using Norma.Eta.Properties;

namespace Norma.Eta.Models.Enums
{
    public static class AbemaChannelExt
    {
        public static string ToIdentifier(string url)
        {
            return url.Replace("https://abema.tv/now-on-air/", "");
        }

        public static string ToLocaleString(string identifier)
        {
            identifier = identifier.Replace("-", "_");
            try
            {
                return (string) typeof(Resources).GetProperty(identifier).GetValue(null);
            }
            catch
            {
                Debug.WriteLine($"WARN: i18n resource key '{identifier}' is not found on resx.");
                return $"##{identifier}##";
            }
        }
    }
}