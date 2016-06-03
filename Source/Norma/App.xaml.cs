using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Threading;

using Norma.Eta;
using Norma.Models;

using I18N = Norma.Eta.Properties.Resources;

namespace Norma
{
    /// <summary>
    ///     App.xaml の相互作用ロジック
    /// </summary>
    // ReSharper disable once RedundantExtendsListEntry
    public partial class App : Application
    {
        #region Overrides of Application

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

#if !DEBUG
            DispatcherUnhandledException += DispatcherOnUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
#endif

            AppInitializer.PreInitialize(this);

            var bootstrap = new Bootstrapper();
            bootstrap.Run();
        }

        #endregion

        // UI スレッド
        // ReSharper disable once UnusedMember.Local
        private void DispatcherOnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            OnUnhandledException(e.Exception);
        }

        // 他
        // ReSharper disable once UnusedMember.Local
        private void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            OnUnhandledException((Exception) e.ExceptionObject);
        }

        private void OnUnhandledException(Exception exception)
        {
            // TODO: ApplicationInsights とかで。
            var path = Path.Combine(NormaConstants.CrashReportsDir,
                                    $"{DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss")}.log");
            var sb = new StringBuilder();
            sb.AppendLine($"==================================================");
            sb.AppendLine($" Crash Report");
            sb.AppendLine($"--------------------------------------------------");
            sb.AppendLine($" Environment Value");
            sb.AppendLine($"  {nameof(ProductInfo.Version)} = {ProductInfo.Version}");
            sb.AppendLine($"  {nameof(NormaConstants.CefCacheDir)} = {NormaConstants.CefCacheDir}");
            sb.AppendLine($"  {nameof(NormaConstants.CefCookiesDir)} = {NormaConstants.CefCookiesDir}");
            sb.AppendLine($"  {nameof(NormaConstants.CrashReportsDir)} = {NormaConstants.CrashReportsDir}");
            sb.AppendLine($"  {nameof(NormaConstants.IsSupportedToast)} = {NormaConstants.IsSupportedToast}");
            sb.AppendLine($"  {nameof(NormaConstants.IsSupportedNewToast)} = {NormaConstants.IsSupportedNewToast}");
            sb.AppendLine();
            sb.AppendLine($" Exception Message");
            sb.AppendLine($"  {exception.Message}");
            sb.AppendLine();
            sb.AppendLine($" Stack Trace");
            sb.AppendLine($"  {exception.StackTrace}");

            using (var sw = new StreamWriter(path))
                sw.WriteLine(sb.ToString());

            sb.Clear();
            sb.AppendLine(I18N.ApplicationHasBeenCrash);
            sb.AppendLine(I18N.PleaseSendCrashReportToMe);
            sb.AppendLine(string.Format(I18N.CrashReportPath, path));

            MessageBox.Show(sb.ToString(), "Norma Crash Report", MessageBoxButton.OK, MessageBoxImage.Error);

            Current.Shutdown();
        }
    }
}