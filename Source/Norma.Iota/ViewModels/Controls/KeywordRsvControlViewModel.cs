using System;
using System.Windows.Input;

using Norma.Eta.Models;
using Norma.Eta.Models.Reservations;
using Norma.Eta.Mvvm;
using Norma.Eta.Notifications;
using Norma.Eta.Validations;
using Norma.Iota.Models;

using Prism.Commands;
using Prism.Interactivity.InteractionRequest;

using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace Norma.Iota.ViewModels.Controls
{
    internal class KeywordRsvControlViewModel : InteractionViewModel<DataPassingNotification>
    {
        private readonly DateTimeValidator _dValidator = new DateTimeValidator();
        private readonly bool _isUpdate;
        private readonly RegexValidator _rgxValidator = new RegexValidator();
        private readonly Reservation _rsv;
        private readonly StringRequiredValidator _srValidator = new StringRequiredValidator();
        private RsvKeyword _model;

        public InteractionRequest<Notification> WindowCloseRequest { get; }
        public InteractionRequest<DataPassingNotification> ResponseRequest { get; }
        public ReactiveProperty<string> ExpiredAt { get; }

        public ReactiveProperty<string> Keyword { get; }

        public ReactiveProperty<bool> IsRegex { get; }

        public ReactiveCommand AddKeywordRsvCommand { get; }

        public KeywordRsvControlViewModel(Reservation reservation, bool isUpdate = true)
        {
            _rsv = reservation;
            _isUpdate = isUpdate;
            WindowCloseRequest = new InteractionRequest<Notification>();
            ResponseRequest = new InteractionRequest<DataPassingNotification>();
            ExpiredAt = new ReactiveProperty<string>(DateTime.MaxValue.ToString("d"))
                .SetValidateNotifyError(w => _dValidator.Validate(w)).AddTo(this);
            IsRegex = new ReactiveProperty<bool>(false).AddTo(this);
            Keyword = new ReactiveProperty<string>()
                .SetValidateNotifyError(w => IsRegex.Value ? _rgxValidator.Validate(w) : _srValidator.Validate(w))
                .AddTo(this);
            AddKeywordRsvCommand = new[]
            {
                ExpiredAt.ObserveHasErrors,
                Keyword.ObserveHasErrors
            }.CombineLatestValuesAreAllFalse().ToReactiveCommand().AddTo(this);
            AddKeywordRsvCommand.Subscribe(w => AddKeywordRsv()).AddTo(this);
            ViewModelHelper.Subscribe(this, w => w.Notification, w =>
            {
                var model = RawNotification.Model;
                if (model is WrapSlot)
                    Keyword.Value = ((WrapSlot) model).Model.Title;
                else if (model is RsvKeyword)
                {
                    var rk = model as RsvKeyword;
                    ExpiredAt.Value = rk.Range.Finish.ToString("g");
                    IsRegex.Value = rk.IsRegexMode;
                    Keyword.Value = rk.Keyword;

                    _model = rk;
                }
                else
                    throw new NotSupportedException();
                IsEnabled = true;
            });
        }

        private void AddKeywordRsv()
        {
            IsEnabled = false;
            if (!_isUpdate)
                _rsv.AddReservation(Keyword.Value, IsRegex.Value,
                                    new DateRange {Finish = _dValidator.Convert(ExpiredAt.Value)});
            else
            {
                _model.Range.Finish = _dValidator.Convert(ExpiredAt.Value);
                _model.IsRegexMode = IsRegex.Value;
                _model.Keyword = Keyword.Value;
                _rsv.UpdateReservation(_model);
            }
            ResponseRequest.Raise(new DataPassingNotification
            {
                Model = new RsvKeyword
                {
                    IsRegexMode = IsRegex.Value,
                    Keyword = Keyword.Value,
                    Range = new DateRange {Finish = _dValidator.Convert(ExpiredAt.Value)}
                }
            }, callback => WindowCloseRequest.Raise(null));
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