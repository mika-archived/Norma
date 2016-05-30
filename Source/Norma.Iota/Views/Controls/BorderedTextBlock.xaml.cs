using System.Windows;
using System.Windows.Controls;

namespace Norma.Iota.Views.Controls
{
    /// <summary>
    ///     BorderedTextBlock.xaml の相互作用ロジック
    /// </summary>
    public partial class BorderedTextBlock : UserControl
    {
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(BorderedTextBlock)
                                        , new FrameworkPropertyMetadata(string.Empty, PropertyChangedCallback));

        public string Text
        {
            get { return (string) GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public BorderedTextBlock()
        {
            InitializeComponent();
        }

        private static void PropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var obj = sender as BorderedTextBlock;
            if (obj != null)
                obj.Text = (string) e.NewValue;
        }
    }
}