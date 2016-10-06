using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Navigation;

namespace Norma.Behaviors
{
    internal class BindingSourceBehavior : Behavior<WebBrowser>
    {
        private void AssociatedObjectOnNavigated(object sender, NavigationEventArgs e) => Source = e.Uri;

        #region Source

        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register(nameof(Source), typeof(Uri), typeof(BindingSourceBehavior),
                                        new PropertyMetadata(null, PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var webBrowser = sender as WebBrowser;
            if (webBrowser != null)
                webBrowser.Source = e.NewValue as Uri;
        }

        public Uri Source
        {
            get { return (Uri) GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        #endregion

        #region Overrides of Behavior

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Navigated += AssociatedObjectOnNavigated;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.Navigated -= AssociatedObjectOnNavigated;
            base.OnDetaching();
        }

        #endregion
    }
}