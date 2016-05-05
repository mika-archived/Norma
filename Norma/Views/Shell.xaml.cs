using System;

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
            Environment.Exit(0); // なぜか終了しない。
        }
    }
}