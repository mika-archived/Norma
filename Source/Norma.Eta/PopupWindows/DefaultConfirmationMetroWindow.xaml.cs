using System.Windows;

using MetroRadiance.UI.Controls;

using Prism.Interactivity.InteractionRequest;

namespace Norma.Eta.PopupWindows
{
    /// <summary>
    ///     DefaultConfirmationMetroWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class DefaultConfirmationMetroWindow : MetroWindow
    {
        public IConfirmation Confirmation
        {
            get { return DataContext as IConfirmation; }
            set { DataContext = value; }
        }

        public DefaultConfirmationMetroWindow()
        {
            InitializeComponent();
        }

        private void OkButton_OnClick(object sender, RoutedEventArgs e)
        {
            Confirmation.Confirmed = true;
            Close();
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            Confirmation.Confirmed = false;
            Close();
        }
    }
}