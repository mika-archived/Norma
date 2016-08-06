using System;
using System.Reactive.Linq;

using Microsoft.Practices.ServiceLocation;

using Norma.Delta.Services;
using Norma.Eta.Mvvm;
using Norma.Eta.Validations;
using Norma.Iota.Models;
using Norma.Iota.ViewModels.WindowContents;

using Reactive.Bindings;

namespace Norma.Iota.ViewModels.Contents
{
    internal class KeywordReservationContentViewModel : ViewModel
    {
        private readonly ReservationService _reservationService;
        private readonly RegexValidator _rgxValidator = new RegexValidator();
        private readonly StringRequiredValidator _srValidator = new StringRequiredValidator();
        public ReactiveProperty<string> Keyword { get; }
        public ReactiveProperty<bool> IsRegexMode { get; }
        public ReactiveCommand RegisterCommand { get; }

        public KeywordReservationContentViewModel(ConditionalReservationContentViewModel viewModel, ReservationItem item)
        {
            _reservationService = ServiceLocator.Current.GetInstance<ReservationService>();
            Keyword = new ReactiveProperty<string>(item?.KeywordReservation.Keyword ?? "").AddTo(this);
            IsRegexMode = new ReactiveProperty<bool>(item?.KeywordReservation.IsRegex ?? false).AddTo(this);
            Keyword.SetValidateNotifyError(w => IsRegexMode.Value ? _rgxValidator.Validate(w) : _srValidator.Validate(w)).AddTo(this);
            RegisterCommand = Keyword.ObserveHasErrors.Select(w => !w).ToReactiveCommand().AddTo(this);
            RegisterCommand.Subscribe(w =>
            {
                if (item == null)
                    _reservationService.InsertKeywordReservation(Keyword.Value, IsRegexMode.Value);
                else
                {
                    item.KeywordReservation.Keyword = Keyword.Value;
                    item.KeywordReservation.IsRegex = IsRegexMode.Value;
                    item.Update();
                }
                viewModel.FinishInteraction.Invoke();
            }).AddTo(this);
        }
    }
}