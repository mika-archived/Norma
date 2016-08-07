using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Threading;

using MetroRadiance.UI;

using Norma.Eta;

namespace Norma.Iota
{
    /// <summary>
    ///     App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        private bool _isHandled;

        #region Overrides of Application

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ThemeService.Current.Register(this, Theme.Dark, Accent.Blue);

#if !DEBUG
            DispatcherUnhandledException += DispatcherOnUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
#endif

            var bootstrap = new Bootstrapper();
            bootstrap.Run();
        }

        #endregion

        // UI スレッド
        // ReSharper disable once UnusedMember.Local
        private void DispatcherOnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            if (!_isHandled)
                OnUnhandledException(e.Exception);
        }

        // 他
        // ReSharper disable once UnusedMember.Local
        private void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (!_isHandled)
                OnUnhandledException((Exception) e.ExceptionObject);
        }

        private void OnUnhandledException(Exception exception)
        {
            _isHandled = true;
            var version = Assembly.GetExecutingAssembly().GetName().Version.ToString();

            var path = Path.Combine(NormaConstants.CrashReportsDir,
                                    $"{DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss")}.log");
            var sb = new StringBuilder();
            sb.AppendLine($"==================================================");
            sb.AppendLine($" Crash Report");
            sb.AppendLine($"--------------------------------------------------");
            sb.AppendLine($"Environment Value");
            sb.AppendLine($"  Version = {version}");
            sb.AppendLine($"  {nameof(NormaConstants.CefCacheDir)} = {NormaConstants.CefCacheDir}");
            sb.AppendLine($"  {nameof(NormaConstants.CefCookiesDir)} = {NormaConstants.CefCookiesDir}");
            sb.AppendLine($"  {nameof(NormaConstants.CrashReportsDir)} = {NormaConstants.CrashReportsDir}");
            sb.AppendLine($"  {nameof(NormaConstants.IsSupportedToast)} = {NormaConstants.IsSupportedToast}");
            sb.AppendLine($"  {nameof(NormaConstants.IsSupportedNewToast)} = {NormaConstants.IsSupportedNewToast}");
            sb.AppendLine();
            sb.AppendLine($"Exception Message");
            sb.AppendLine($"  {exception.Message}");
            sb.AppendLine();
            sb.AppendLine($"Stack Trace");
            sb.AppendLine($"{exception.StackTrace}");

            using (var sw = new StreamWriter(path))
                sw.WriteLine(sb.ToString());

            sb.Clear();
            sb.AppendLine(Eta.Properties.Resources.ApplicationHasBeenCrash);
            sb.AppendLine(Eta.Properties.Resources.PleaseSendCrashReportToMe);
            sb.AppendLine(string.Format(Eta.Properties.Resources.CrashReportPath, path));

            MessageBox.Show(sb.ToString(), "Norma.Iota Crash Report", MessageBoxButton.OK, MessageBoxImage.Error);

            Current.Shutdown();
        }
    }
}