using System;
using System.Reactive.Linq;

using Microsoft.Practices.ServiceLocation;

using Norma.Delta.Services;
using Norma.Eta.Mvvm;
using Norma.Eta.Validations;
using Norma.Iota.ViewModels.WindowContents;

using Reactive.Bindings;

namespace Norma.Iota.ViewModels.Contents
{
    internal class KeywordReservationContentViewModel : ViewModel
    {
        private readonly ReservationService _reservationService;
        private readonly RegexValidator _rgxValidator = new RegexValidator();
        private readonly StringRequiredValidator _srValidator = new StringRequiredValidator();
        private readonly ConditionalReservationContentViewModel _viewModel;
        public ReactiveProperty<string> Keyword { get; }
        public ReactiveProperty<bool> IsRegexMode { get; }
        public ReactiveCommand RegisterCommand { get; }

        public KeywordReservationContentViewModel(ConditionalReservationContentViewModel viewModel)
        {
            _viewModel = viewModel;
            _reservationService = ServiceLocator.Current.GetInstance<ReservationService>();
            Keyword = new ReactiveProperty<string>();
            IsRegexMode = new ReactiveProperty<bool>();
            Keyword.SetValidateNotifyError(w => IsRegexMode.Value ? _rgxValidator.Validate(w) : _srValidator.Validate(w));
            RegisterCommand = Keyword.ObserveHasErrors.Select(w => !w).ToReactiveCommand();
            RegisterCommand.Subscribe(w =>
            {
                _reservationService.InsertKeywordReservation(Keyword.Value, IsRegexMode.Value);
                viewModel.WindowCloseRequest.Raise(null);
            });
        }
    }
}