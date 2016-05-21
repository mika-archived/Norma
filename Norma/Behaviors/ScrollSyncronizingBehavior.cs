using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

using Orientation = Norma.Models.Orientation;

namespace Norma.Behaviors
{
    // スクロール位置を同期します。
    internal class ScrollSyncronizingBehavior : Behavior<ScrollViewer>
    {
        public static readonly DependencyProperty TargetNameProperty =
            DependencyProperty.Register(nameof(TargetName), typeof(string), typeof(ScrollSyncronizingBehavior));

        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register(nameof(Orientation), typeof(Orientation), typeof(ScrollSyncronizingBehavior),
                                        new PropertyMetadata(Orientation.Both));

        public static readonly DependencyProperty UpstreamLevelProperty =
            DependencyProperty.Register(nameof(UpstreamLevel), typeof(int), typeof(ScrollSyncronizingBehavior));

        public Orientation Orientation
        {
            get { return (Orientation) GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        public string TargetName
        {
            get { return (string) GetValue(TargetNameProperty); }
            set { SetValue(TargetNameProperty, value); }
        }

        public int UpstreamLevel
        {
            get { return (int) GetValue(UpstreamLevelProperty); }
            set { SetValue(UpstreamLevelProperty, value); }
        }

        private void AssociatedObjectOnScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var element = GetTargetElement() as ScrollViewer;
            if (Orientation == Orientation.Horizontal || Orientation == Orientation.Both)
                element?.ScrollToHorizontalOffset(e.HorizontalOffset);
            if (Orientation == Orientation.Vertical || Orientation == Orientation.Both)
                element?.ScrollToVerticalOffset(e.VerticalOffset);
        }

        // 同等レベル以下の TargetName Element を取得
        private FrameworkElement GetTargetElement()
        {
            var current = (FrameworkElement) AssociatedObject;
            for (var i = 0; i < UpstreamLevel; i++)
                current = (FrameworkElement) current.Parent;
            return (FrameworkElement) current.FindName(TargetName);
        }

        #region Overrides of Behavior

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.ScrollChanged += AssociatedObjectOnScrollChanged;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.ScrollChanged -= AssociatedObjectOnScrollChanged;
            base.OnDetaching();
        }

        #endregion
    }
}