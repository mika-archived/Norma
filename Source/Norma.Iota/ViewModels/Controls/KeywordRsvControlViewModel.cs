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
        private readonly DateTimeValidator _dtValidator = new DateTimeValidator(true);
        private readonly DateTimeValidator _dValidator = new DateTimeValidator();
        private readonly bool _isUpdate;
        private readonly RegexValidator _rgxValidator = new RegexValidator();
        private readonly Reservation _rsv;
        private readonly StringRequiredValidator _srValidator = new StringRequiredValidator();

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
            ViewModelHelper.Subscribe(this, nameof(Notification), w =>
            {
                var model = RawNotification.Model;
                if (model is WrapSlot)
                    Keyword.Value = ((WrapSlot) model).Model.Title;
                else if (model is RsvKeyword)
                {
                    var rt = model as RsvKeyword;
                    ExpiredAt.Value = rt.Range.Finish.ToString("g");
                    IsRegex.Value = rt.IsRegexMode;
                    Keyword.Value = rt.Keyword;
                }
                else
                    return;
            });
        }

        private void AddKeywordRsv()
        {
            if (!_isUpdate)
                _rsv.AddReservation(Keyword.Value, IsRegex.Value,
                                    new DateRange {Finish = _dValidator.Convert(ExpiredAt.Value)});
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

        #region CancelCommand

        private ICommand _cancelCommand;

        public ICommand CancelCommand => _cancelCommand ?? (_cancelCommand = new DelegateCommand(Cancel));

        private void Cancel() => WindowCloseRequest.Raise(null);

        #endregion
    }
}