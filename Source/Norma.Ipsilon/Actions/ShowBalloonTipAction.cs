using System.Windows;
using System.Windows.Interactivity;

using Hardcodet.Wpf.TaskbarNotification;

using Norma.Eta;

using Prism.Interactivity.InteractionRequest;

namespace Norma.Ipsilon.Actions
{
    internal class ShowBalloonTipAction : TriggerAction<TaskbarIcon>
    {
        public static readonly DependencyProperty BalloonIconProperty
            = DependencyProperty.Register(nameof(BalloonIcon), typeof(BalloonIcon), typeof(ShowBalloonTipAction),
                                          new PropertyMetadata(BalloonIcon.Info));

        public BalloonIcon BalloonIcon
        {
            get { return (BalloonIcon) GetValue(BalloonIconProperty); }
            set { SetValue(BalloonIconProperty, value); }
        }

        #region Overrides of TriggerAction

        protected override void Invoke(object parameter)
        {
            var args = parameter as InteractionRequestedEventArgs;
            if (args == null)
                return;
            var notification = args.Context;

            if (NormaConstants.NotSupportedVersion)
                AssociatedObject.ShowBalloonTip(notification.Title, notification.Content.ToString(), BalloonIcon);
        }

        #endregion
    }
}