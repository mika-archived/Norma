using System;
using System.Diagnostics;

using Norma.Eta.Properties;

namespace Norma.Eta.Extensions
{
    public static class EnumExt
    {
        public static string ToLocaleString(this Enum @enum)
        {
            var identifier = @enum.ToString();
            try
            {
                return (string) typeof(Resources).GetProperty(identifier).GetValue(null);
            }
            catch
            {
                Debug.WriteLine($"WARN: i18n resource key '{identifier}' is not found on resx.");
                return $"{identifier}";
            }
        }
    }
}