using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace Norma.Behaviors
{
    // OneWayToSource
    internal class BindingWebBrowserBehavior : Behavior<WebBrowser>
    {
        #region Overrides of Behavior

        protected override void OnAttached()
        {
            base.OnAttached();
            WebBrowser = AssociatedObject;
        }

        #endregion

        #region WebBrowser

        public static readonly DependencyProperty WebBrowserProperty =
            DependencyProperty.Register(nameof(WebBrowser), typeof(WebBrowser), typeof(BindingWebBrowserBehavior),
                                        new PropertyMetadata(null));

        public WebBrowser WebBrowser
        {
            get { return (WebBrowser) GetValue(WebBrowserProperty); }
            set { SetValue(WebBrowserProperty, value); }
        }

        #endregion
    }
}