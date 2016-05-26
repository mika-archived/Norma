using System;
using System.Collections.ObjectModel;

using Norma.Eta.Mvvm;
using Norma.Eta.Notifications;
using Norma.Eta.Properties;
using Norma.Iota.Models;

using Prism.Interactivity.InteractionRequest;

namespace Norma.Iota.ViewModels.WindowContents
{
    internal class SlotDetailsContentViewModel : ViewModel, IInteractionRequestAware
    {
        public ObservableCollection<string> Cast { get; }
        public ObservableCollection<string> Staff { get; }

        public SlotDetailsContentViewModel()
        {
            Cast = new ObservableCollection<string>();
            Staff = new ObservableCollection<string>();
            ViewModelHelper.Subscribe(this, nameof(Notification), w =>
            {
                var model = _notification.Model as WrapSlot;
                if (model == null)
                    return;
                WindowTitle = $"{model.Model.Title} - {Resources.ProgramDetails} - Norma";
                Title = model.Model.Title;
                Date = model.StartAt.ToString("MM/DD");
                Time = $"{model.Model.StartAt.ToString("MM/dd HH:mm")} ～ {model.Model.EndAt.ToString("MM/dd HH:mm")}";
                Description = model.DetailHighlight;
                model.Cast?.ForEach(x => Cast.Add(x));
                model.Staff?.ForEach(x => Staff.Add(x));
                Thumbnail = $"https://hayabusa.io/abema/programs/{model.Model.DisplayProgramId}/thumb001.w200.h112.jpg";
            }).AddTo(this);
        }

        #region WindowTitle

        private string _windowTitle;

        public string WindowTitle
        {
            get { return _windowTitle; }
            set { SetProperty(ref _windowTitle, value); }
        }

        #endregion

        #region Title

        private string _title;

        public string Title
        {
            get { return _title; }
            private set { SetProperty(ref _title, value); }
        }

        #endregion

        #region Date

        private string _date;

        public string Date
        {
            get { return _date; }
            private set { SetProperty(ref _date, value); }
        }

        #endregion

        #region Time

        private string _time;

        public string Time
        {
            get { return _time; }
            private set { SetProperty(ref _time, value); }
        }

        #endregion

        #region Description

        private string _description;

        public string Description
        {
            get { return _description; }
            private set { SetProperty(ref _description, value); }
        }

        #endregion

        #region Thumbnail

        private string _thumbnail;

        public string Thumbnail
        {
            get { return _thumbnail; }
            set { SetProperty(ref _thumbnail, value); }
        }

        #endregion

        #region Implementation of IInteractionRequestAware

        #region Notification

        private DataPassingNotification _notification;

        public INotification Notification
        {
            get { return _notification; }
            set
            {
                var notification = value as DataPassingNotification;
                if (notification == null)
                    return;
                _notification = notification;
                OnPropertyChanged();
            }
        }

        #endregion

        public Action FinishInteraction { get; set; }

        #endregion
    }
}