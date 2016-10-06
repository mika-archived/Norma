using System.Windows.Controls;
using System.Windows.Interactivity;

using Norma.Models.Browser;

namespace Norma.Behaviors
{
    internal class CaptureBrowserNetworkBehavior : Behavior<WebBrowser>
    {
        private HttpResourceDistributor _distributor;

        #region Overrides of Behavior

        protected override void OnAttached()
        {
            base.OnAttached();
            _distributor = new HttpResourceDistributor(AssociatedObject);
        }

        #endregion
    }
}