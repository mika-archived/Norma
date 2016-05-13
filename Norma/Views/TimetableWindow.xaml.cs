using System;

using MetroRadiance.UI.Controls;

namespace Norma.Views
{
    /// <summary>
    ///     TimetableWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class TimetableWindow : MetroWindow
    {
        public TimetableWindow()
        {
            InitializeComponent();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Close();
        }
    }
}