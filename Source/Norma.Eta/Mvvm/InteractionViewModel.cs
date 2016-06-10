using System;

using Prism.Interactivity.InteractionRequest;

namespace Norma.Eta.Mvvm
{
    public class InteractionViewModel<T> : ViewModel, IInteractionRequestAware where T : class, INotification
    {
        #region Implementation of IInteractionRequestAware

        #region Notification

        protected T RawNotification;

        public INotification Notification
        {
            get { return RawNotification; }
            set
            {
                var notification = value as T;
                if (notification == null)
                    return;
                RawNotification = notification;
                OnPropertyChanged();
            }
        }

        #endregion

        public Action FinishInteraction { get; set; }

        #endregion
    }
}