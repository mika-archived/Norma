using System;

namespace Norma.Models
{
    internal enum ReleaseType
    {
        Release,

        Preview
    }

    internal static class ReleaseTypeExt
    {
        internal static string ToVersionString(this ReleaseType releaseType)
        {
            switch (releaseType)
            {
                case ReleaseType.Release:
                    return "";

                case ReleaseType.Preview:
                    return releaseType.ToString();

                default:
                    throw new ArgumentOutOfRangeException(nameof(releaseType), releaseType, null);
            }
        }
    }
}