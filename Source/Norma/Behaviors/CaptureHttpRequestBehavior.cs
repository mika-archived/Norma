using System.Windows.Interactivity;

using CefSharp.Wpf;

using Microsoft.Practices.ServiceLocation;

using Norma.Eta.Models;
using Norma.Models.Browser;

namespace Norma.Behaviors
{
    internal class CaptureHttpRequestBehavior : Behavior<ChromiumWebBrowser>
    {
        private readonly Configuration _configuration;

        public CaptureHttpRequestBehavior()
        {
            _configuration = ServiceLocator.Current.GetInstance<Configuration>();
        }

        #region Overrides of Behavior

        protected override void OnAttached()
        {
            base.OnAttached();
            if (_configuration.Root.Others.IsEnabledExperimentalFeatures)
                AssociatedObject.ResourceHandlerFactory = new HttpResourceHandlerFactory();
        }

        #endregion
    }
}