using System;
using System.Windows;
using System.Windows.Interactivity;

namespace Norma.Behaviors
{
    // Target に指定された子要素の比を 16:9 に保つ
    internal class KeepAspectRatioBehavior : Behavior<FrameworkElement>
    {
        public static readonly DependencyProperty TargetProperty =
            DependencyProperty.Register(nameof(Target), typeof(string), typeof(KeepAspectRatioBehavior));

        public string Target
        {
            get { return (string) GetValue(TargetProperty); }
            set { SetValue(TargetProperty, value); }
        }

        #region Overrides of Behavior

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.SizeChanged += AssociatedObjectOnSizeChanged;
        }

        private void AssociatedObjectOnSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
        {
            var height = sizeChangedEventArgs.NewSize.Height;
            var width = sizeChangedEventArgs.NewSize.Width;
            var element = GetChildElement();
            Tuple<double, double> size;
            if (height > width)
                size = GetAspectRatioSize(height, width, 0);
            else if (width > height)
                size = GetAspectRatioSize(height, width, 1);
            else
                size = new Tuple<double, double>(height, width);
            if (height < size.Item1)
                size = GetAspectRatioSize(height, width, 0);
            else if (width < size.Item2)
                size = GetAspectRatioSize(height, width, 1);
            element.Height = size.Item1;
            element.Width = size.Item2;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.SizeChanged -= AssociatedObjectOnSizeChanged;
            base.OnDetaching();
        }

        private FrameworkElement GetChildElement()
        {
            return (FrameworkElement) ((FrameworkElement) AssociatedObject.Parent).FindName(Target);
        }

        // flag = 0 : height, flag = 1 : width
        private Tuple<double, double> GetAspectRatioSize(double height, double width, int flag)
        {
            return flag == 0
                ? new Tuple<double, double>(height, height / 9 * 16)
                : new Tuple<double, double>(width / 16 * 9, width);
        }

        #endregion
    }
}