using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace Norma.Behaviors
{
    internal class HorizontalScrollBehavior : Behavior<ScrollViewer>
    {
        #region Overrides of Behavior

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PreviewMouseWheel += AssociatedObjectOnPreviewMouseWheel;
        }

        private void AssociatedObjectOnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var scrollViewer = sender as ScrollViewer;
            if (e.Delta > 0)
                scrollViewer?.LineLeft();
            else
                scrollViewer?.LineRight();
            e.Handled = true;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.PreviewMouseWheel -= AssociatedObjectOnPreviewMouseWheel;
            base.OnDetaching();
        }

        #endregion
    }
}