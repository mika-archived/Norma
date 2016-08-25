using System;

namespace Norma.Eta.Models.Enums
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
    }
}