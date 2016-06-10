using Hardcodet.Wpf.TaskbarNotification;

namespace Norma.Ipsilon.Views
{
    /// <summary>
    ///     Shell.xaml の相互作用ロジック
    /// </summary>
    public partial class Shell : TaskbarIcon
    {
        public static TaskbarIcon TaskbarIcon { get; private set; }

        public Shell()
        {
            InitializeComponent();
            TaskbarIcon = this;
        }
    }
}