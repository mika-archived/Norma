using System.Windows;

using MetroRadiance.UI.Controls;

using Prism.Interactivity.InteractionRequest;

namespace Norma.Eta.PopupWindows
{
    /// <summary>
    ///     DefaultNotificationMetroWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class DefaultNotificationMetroWindow : MetroWindow
    {
        /// <summary>
        ///     Sets or gets the <see cref="INotification" /> shown by this window./>
        /// </summary>
        public INotification Notification
        {
            get { return DataContext as INotification; }
            set { DataContext = value; }
        }

        public DefaultNotificationMetroWindow()
        {
            InitializeComponent();
        }

        private void OKButton_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}