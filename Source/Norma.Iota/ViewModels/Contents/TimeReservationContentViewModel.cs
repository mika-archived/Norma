using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;

using Microsoft.Practices.ServiceLocation;

using Norma.Delta.Models.Enums;
using Norma.Delta.Services;
using Norma.Eta.Models;
using Norma.Eta.Mvvm;
using Norma.Eta.Validations;
using Norma.Iota.Models;
using Norma.Iota.ViewModels.WindowContents;

using Reactive.Bindings;

namespace Norma.Iota.ViewModels.Contents
{
    internal class TimeReservationContentViewModel : ViewModel
    {
        private readonly DateTimeValidator _dtValidator = new DateTimeValidator(true);
        private readonly ReservationService _reservationService;

        public List<EnumWrap<Repetition>> RepetitionTypes
            => ((Repetition[]) Enum.GetValues(typeof(Repetition))).Select(w => new EnumWrap<Repetition>(w)).ToList();

        public ReactiveProperty<string> StartAt { get; }
        public ReactiveProperty<EnumWrap<Repetition>> RepetitionType { get; }
        public ReactiveCommand RegisterCommand { get; }

        public TimeReservationContentViewModel(ConditionalReservationContentViewModel viewModel, ReservationItem item)
        {
            _reservationService = ServiceLocator.Current.GetInstance<ReservationService>();
            StartAt = new ReactiveProperty<string>(item?.StartAt.ToString() ?? "").AddTo(this);
            RepetitionType = new ReactiveProperty<EnumWrap<Repetition>>(new EnumWrap<Repetition>(Repetition.None)).AddTo(this);
            StartAt.SetValidateNotifyError(w => _dtValidator.Validate(w)).AddTo(this);
            RegisterCommand = StartAt.ObserveHasErrors.Select(w => !w).ToReactiveCommand().AddTo(this);
            RegisterCommand.Subscribe(w =>
            {
                if (item == null)
                    _reservationService.InsertTimeReservaion(DateTime.Parse(StartAt.Value),
                                                             RepetitionType.Value.EnumValue);
                else
                {
                    item.TimeReservation.StartAt = DateTime.Parse(StartAt.Value);
                    item.Update();
                }
                viewModel.FinishInteraction.Invoke();
            }).AddTo(this);
        }
    }
}