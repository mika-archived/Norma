using System;

namespace Norma.Eta.Models
{
    public enum VideoQuality
    {
        Lowest,

        Lower,

        Medium,

        Higher,

        Highest,

        Auto
    }

    public static class VideoQualityExt
    {
        public static string ToProgressive(this VideoQuality quality)
        {
            switch (quality)
            {
                case VideoQuality.Lowest:
                    return "240";

                case VideoQuality.Lower:
                    return "360";

                case VideoQuality.Medium:
                    return "480";

                case VideoQuality.Higher:
                    return "720";

                case VideoQuality.Highest:
                    return "1080";

                default:
                    throw new ArgumentOutOfRangeException(nameof(quality), quality, null);
            }
        }

        public static string ToLocaleString(this VideoQuality quality)
        {
            switch (quality)
            {
                case VideoQuality.Lowest:
                    return "Lowest";

                case VideoQuality.Lower:
                    return "Lower";

                case VideoQuality.Medium:
                    return "Medium";

                case VideoQuality.Higher:
                    return "Higer";

                case VideoQuality.Highest:
                    return "Highest";

                case VideoQuality.Auto:
                    return "Auto";

                default:
                    throw new ArgumentOutOfRangeException(nameof(quality), quality, null);
            }
        }
    }
}