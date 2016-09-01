using Microsoft.Win32;

using Norma.Eta;

namespace Norma.Models.Browser
{
    internal class BrowserEmuration
    {
        public static void Register()
        {
            using (var reg = Registry.CurrentUser.OpenSubKey(FeatureBrowserEmuration, true))
            {
                reg?.SetValue(NormaConstants.MainExecutableFile, 11001);
            }
            using (var reg = Registry.CurrentUser.OpenSubKey(FeatureGpuRendering, true))
            {
                reg?.SetValue(NormaConstants.MainExecutableFile, 1);
            }
            using (var reg = Registry.CurrentUser.OpenSubKey(FeatureMaxConnection10))
            {
                reg?.SetValue(NormaConstants.MainExecutableFile, 10);
            }
            using (var reg = Registry.CurrentUser.OpenSubKey(FeatureMaxConnection11))
            {
                reg?.SetValue(NormaConstants.MainExecutableFile, 10);
            }
            using (var reg = Registry.CurrentUser.OpenSubKey(FeatureAlignedTimers))
            {
                reg?.SetValue(NormaConstants.MainExecutableFile, 1);
            }
            using (var reg = Registry.CurrentUser.OpenSubKey(FeatureAllowHighfreqTimers))
            {
                reg?.SetValue(NormaConstants.MainExecutableFile, 1);
            }
            using (var reg = Registry.CurrentUser.OpenSubKey(FeatureUseLegacyJscript))
            {
                reg?.SetValue(NormaConstants.MainExecutableFile, 0);
            }
        }

        #region Keys

        private static readonly string _baseRegistry = @"Software\Microsoft\Internet Explorer\Main\FeatureControl\";

        // IE バージョン
        private static readonly string FeatureBrowserEmuration = _baseRegistry + "FEATURE_BROWSER_EMULATION";

        // GPU 支援
        private static readonly string FeatureGpuRendering = _baseRegistry + "FEATURE_GPU_RENDERING";

        // 同時接続数 (HTTP/1)
        private static readonly string FeatureMaxConnection10 = _baseRegistry + "FEATURE_MAXCONNECTIONSPER1_0SERVER";

        // 同時接続数 (HTTP/1.1)
        private static readonly string FeatureMaxConnection11 = _baseRegistry + "FEATURE_MAXCONNECTIONSPERSERVER";

        // 省電力モード解除
        private static readonly string FeatureAlignedTimers = _baseRegistry + "FEATURE_ALIGNED_TIMERS";

        // 省電力モード解除
        private static readonly string FeatureAllowHighfreqTimers = _baseRegistry + "FEATURE_ALLOW_HIGHFREQ_TIMERS";

        // Chakra 使用
        private static readonly string FeatureUseLegacyJscript = _baseRegistry + "FEATURE_USE_LEGACY_JSCRIPT";

        #endregion
    }
}