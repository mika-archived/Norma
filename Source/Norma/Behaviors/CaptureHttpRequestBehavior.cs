using System.Windows.Interactivity;

using CefSharp.Wpf;

using Norma.Models.Browser;

namespace Norma.Behaviors
{
    internal class CaptureHttpRequestBehavior : Behavior<ChromiumWebBrowser>
    {
        public static bool IsEnabledCapture { get; set; }

        #region Overrides of Behavior

        protected override void OnAttached()
        {
            base.OnAttached();
            if (IsEnabledCapture)
                AssociatedObject.ResourceHandlerFactory = new HttpResourceHandlerFactory();
        }

        #endregion
    }
}