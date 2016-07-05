using System.Windows.Interactivity;

using CefSharp.Wpf;

using Norma.Models.Browser;

namespace Norma.Behaviors
{
    internal class CaptureHttpRequestBehavior : Behavior<ChromiumWebBrowser>
    {
        #region Overrides of Behavior

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.ResourceHandlerFactory = new HttpResourceHandlerFactory();
        }

        #endregion
    }
}