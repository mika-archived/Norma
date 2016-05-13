using System;
using System.Windows;

using MetroRadiance.UI.Controls;

namespace Norma.Views
{
    /// <summary>
    ///     Shell.xaml の相互作用ロジック
    /// </summary>
    public partial class Shell : MetroWindow
    {
        public Shell()
        {
            InitializeComponent();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.Shutdown(); // なぜか終了しない。
        }
    }
}