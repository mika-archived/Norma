using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

using Norma.Eta.Models;
using Norma.Eta.Models.Reservations;
using Norma.Eta.Mvvm;
using Norma.Eta.Notifications;
using Norma.Eta.Properties;
using Norma.Eta.Validations;
using Norma.Iota.Models;

using Prism.Commands;
using Prism.Interactivity.InteractionRequest;

using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace Norma.Iota.ViewModels.WindowContents
{
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class DetailsRsvContentViewModel : ViewModel, IInteractionRequestAware
    {
        private readonly DateTimeValidator _dtValidator = new DateTimeValidator(true);
        private readonly DateTimeValidator _dValidator = new DateTimeValidator();
        private readonly RegexValidator _rgxValidator = new RegexValidator();
        private readonly Reservation _rsv;
        private readonly StringRequiredValidator _srValidator = new StringRequiredValidator();

        public List<EnumWrap<RepetitionType>> RepetitionTypes
            => ((RepetitionType[]) Enum.GetValues(typeof(RepetitionType))).Select(w => new EnumWrap<RepetitionType>(w))
                                                                          .ToList();

        public InteractionRequest<Notification> WindowCloseRequest { get; }

        // 共通
        public ReactiveProperty<string> StartAt { get; }

        public ReactiveProperty<string> ExpiredAt { get; }

        // Time
        public ReactiveProperty<EnumWrap<RepetitionType>> Repetition { get; }

        // Keyword
        public ReactiveProperty<string> Keyword { get; }

        public ReactiveProperty<bool> IsRegex { get; }

        public ReactiveCommand AddTimeRsvCommand { get; }
        public ReactiveCommand AddKeywordRsvCommand { get; }

        public DetailsRsvContentViewModel(Reservation reservation)
        {
            _rsv = reservation;
            WindowCloseRequest = new InteractionRequest<Notification>();
            StartAt = new ReactiveProperty<string>(DateTime.Now.ToString("g"))
                .SetValidateNotifyError(w => _dtValidator.Validate(w)).AddTo(this);
            ExpiredAt = new ReactiveProperty<string>(DateTime.MaxValue.ToString("d"))
                .SetValidateNotifyError(w => _dValidator.Validate(w)).AddTo(this);
            var defValue = new EnumWrap<RepetitionType>(RepetitionType.None);
            Repetition = new ReactiveProperty<EnumWrap<RepetitionType>>(defValue).AddTo(this);
            IsRegex = new ReactiveProperty<bool>(false).AddTo(this);
            Keyword = new ReactiveProperty<string>()
                .SetValidateNotifyError(w => IsRegex.Value ? _rgxValidator.Validate(w) : _srValidator.Validate(w))
                .AddTo(this);
            AddTimeRsvCommand = new[]
            {
                StartAt.ObserveHasErrors,
                ExpiredAt.ObserveHasErrors
            }.CombineLatestValuesAreAllFalse().ToReactiveCommand().AddTo(this);
            AddTimeRsvCommand.Subscribe(w => AddTimeRsv()).AddTo(this);
            AddKeywordRsvCommand = new[]
            {
                ExpiredAt.ObserveHasErrors,
                Keyword.ObserveHasErrors
            }.CombineLatestValuesAreAllFalse().ToReactiveCommand().AddTo(this);
            AddKeywordRsvCommand.Subscribe(w => AddKeywordRsv()).AddTo(this);

            ViewModelHelper.Subscribe(this, nameof(Notification), w =>
            {
                var model = _notification.Model as WrapSlot;
                if (model == null)
                    return;
                WindowTitle = $"{model.Model.Title} - {Resources.ViewingDRsv} - Norma";
                StartAt.Value = model.StartAt.ToString("g");
            }).AddTo(this);
        }

        private void AddTimeRsv()
        {
            _rsv.AddReservation(_dtValidator.Convert(StartAt.Value), Repetition.Value.EnumValue,
                                new DateRange {Finish = _dValidator.Convert(ExpiredAt.Value)});
            WindowCloseRequest.Raise(null);
        }

        private void AddKeywordRsv()
        {
            _rsv.AddReservation(Keyword.Value, IsRegex.Value,
                                new DateRange {Finish = _dValidator.Convert(ExpiredAt.Value)});
            WindowCloseRequest.Raise(null);
        }

        #region WindowTitle

        private string _windowTitle;

        public string WindowTitle
        {
            get { return _windowTitle; }
            set { SetProperty(ref _windowTitle, value); }
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

        #region CancelCommand

        private ICommand _cancelCommand;

        public ICommand CancelCommand => _cancelCommand ?? (_cancelCommand = new DelegateCommand(Cancel));

        private void Cancel() => WindowCloseRequest.Raise(null);

        #endregion
    }
}