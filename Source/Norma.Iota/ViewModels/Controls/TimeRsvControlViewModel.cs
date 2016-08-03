using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

using Norma.Delta.Models.Enums;
using Norma.Delta.Services;
using Norma.Eta.Models;
using Norma.Eta.Mvvm;
using Norma.Eta.Notifications;
using Norma.Eta.Validations;

using Prism.Commands;
using Prism.Interactivity.InteractionRequest;

using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace Norma.Iota.ViewModels.Controls
{
    internal class TimeRsvControlViewModel : InteractionViewModel<DataPassingNotification>
    {
        private readonly DateTimeValidator _dtValidator = new DateTimeValidator(true);
        private readonly DateTimeValidator _dValidator = new DateTimeValidator();
        private readonly bool _isUpdate;
        private readonly ReservationService _reservationService;
        // private RsvTime _model;

        public List<EnumWrap<Repetition>> RepetitionTypes
            => ((Repetition[]) Enum.GetValues(typeof(Repetition))).Select(w => new EnumWrap<Repetition>(w))
                                                                  .ToList();

        public InteractionRequest<Notification> WindowCloseRequest { get; }
        public InteractionRequest<DataPassingNotification> ResponseRequest { get; }

        public ReactiveProperty<string> StartAt { get; }

        public ReactiveProperty<string> ExpiredAt { get; }

        public ReactiveProperty<EnumWrap<Repetition>> Repetition { get; }

        public ReactiveCommand AddTimeRsvCommand { get; }

        public TimeRsvControlViewModel(ReservationService reservationService, bool isUpdate = true)
        {
            _reservationService = reservationService;
            _isUpdate = isUpdate;
            WindowCloseRequest = new InteractionRequest<Notification>();
            ResponseRequest = new InteractionRequest<DataPassingNotification>();
            StartAt = new ReactiveProperty<string>(DateTime.Now.ToString("g"))
                .SetValidateNotifyError(w => _dtValidator.Validate(w)).AddTo(this);
            ExpiredAt = new ReactiveProperty<string>(DateTime.MaxValue.ToString("d"))
                .SetValidateNotifyError(w => _dValidator.Validate(w)).AddTo(this);
            var defValue = new EnumWrap<Repetition>(Delta.Models.Enums.Repetition.None);
            Repetition = new ReactiveProperty<EnumWrap<Repetition>>(defValue).AddTo(this);
            AddTimeRsvCommand = new[]
            {
                StartAt.ObserveHasErrors,
                ExpiredAt.ObserveHasErrors
            }.CombineLatestValuesAreAllFalse().ToReactiveCommand().AddTo(this);
            AddTimeRsvCommand.Subscribe(w => AddTimeRsv()).AddTo(this);
            ViewModelHelper.Subscribe(this, w => w.Notification, w =>
            {
                /*
                var model = RawNotification.Model;
                if (model is WrapSlot)
                    StartAt.Value = ((WrapSlot) model).StartAt.ToString("g");
                else if (model is RsvTime)
                {
                    var rt = model as RsvTime;
                    StartAt.Value = rt.StartTime.ToString("g");
                    ExpiredAt.Value = rt.Range.Finish.ToString("g");
                    Repetition.Value = new EnumWrap<RepetitionType>(rt.DayOfWeek);

                    _model = rt;
                }
                IsEnabled = true;
                */
            });
        }

        private void AddTimeRsv()
        {
            /*
            IsEnabled = false;
            if (!_isUpdate)
                _rsv.AddReservation(_dtValidator.Convert(StartAt.Value), Repetition.Value.EnumValue,
                                    new DateRange {Finish = _dValidator.Convert(ExpiredAt.Value)});
            else
            {
                _model.StartTime = _dtValidator.Convert(StartAt.Value);
                _model.DayOfWeek = Repetition.Value.EnumValue;
                _model.Range.Finish = _dValidator.Convert(ExpiredAt.Value);
                _rsv.UpdateReservation(_model);
            }
            ResponseRequest.Raise(new DataPassingNotification
            {
                Model = new RsvTime
                {
                    StartTime = _dtValidator.Convert(StartAt.Value),
                    Range = new DateRange {Finish = _dValidator.Convert(ExpiredAt.Value)}
                }
            }, callback => WindowCloseRequest.Raise(null));
            */
        }

        #region IsEnabled

        private bool _isEnabled;

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set { SetProperty(ref _isEnabled, value); }
        }

        #endregion

        #region CancelCommand

        private ICommand _cancelCommand;

        public ICommand CancelCommand => _cancelCommand ?? (_cancelCommand = new DelegateCommand(Cancel));

        private void Cancel() => WindowCloseRequest.Raise(null);

        #endregion
    }
}