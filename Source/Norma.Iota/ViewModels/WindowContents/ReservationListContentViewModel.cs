using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Windows.Input;

using Norma.Delta.Models;
using Norma.Delta.Services;
using Norma.Eta.Mvvm;
using Norma.Eta.Notifications;
using Norma.Eta.Properties;
using Norma.Iota.Models;

using Prism.Commands;
using Prism.Interactivity.InteractionRequest;

using Reactive.Bindings;

namespace Norma.Iota.ViewModels.WindowContents
{
    internal class ReservationListContentViewModel : InteractionViewModel<DataPassingNotification>
    {
        private readonly DatabaseService _databaseService;
        public string WindowTitle => Resources.RsvList;

        public ObservableCollection<ReservationItemViewModel> Reservations { get; }
        public ReactiveProperty<ReservationItemViewModel> SelectedItem { get; }
        public InteractionRequest<Confirmation> ConfirmationRequest { get; }

        public InteractionRequest<DataPassingNotification> ConditionalReservationRequest { get; }

        public ReservationListContentViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
            Reservations = new ObservableCollection<ReservationItemViewModel>();
            SelectedItem = new ReactiveProperty<ReservationItemViewModel>();
            ConfirmationRequest = new InteractionRequest<Confirmation>();
            ConditionalReservationRequest = new InteractionRequest<DataPassingNotification>();
            SelectedItem.Subscribe(w =>
            {
                EditReservationCommand.RaiseCanExecuteChanged();
                DeleteReservationCommand.RaiseCanExecuteChanged();
            }).AddTo(this);
            ViewModelHelper.Subscribe(this, w => w.Notification, w => UpdateRsvList());
        }

        private void UpdateRsvList()
        {
            Reservations.Clear();
            List<Reservation> reservations;
            using (var connection = _databaseService.Connect())
                reservations = connection.Reservations.Where(w => w.IsEnabled)
                                         .Include(w => w.KeywordReservation)
                                         .Include(w => w.SeriesReservation.Series.Episodes)
                                         .Include(w => w.SlotReservation.Slot)
                                         .Include(w => w.SlotReservation2)
                                         .Include(w => w.TimeReservation)
                                         .ToList();
            foreach (var reservation in reservations)
                Reservations.Add(new ReservationItemViewModel(new ReservationItem(reservation)));
        }

        #region RegisterReservationCommand

        private ICommand _registerReservationCommand;

        public ICommand RegisterReservationCommand
            => _registerReservationCommand ?? (_registerReservationCommand = new DelegateCommand(RegisterReservation));

        private void RegisterReservation()
        {
            ConditionalReservationRequest.Raise(new DataPassingNotification {Title = Resources.Register});
            UpdateRsvList();
        }

        #endregion

        #region EditReservationCommand

        private DelegateCommand _editRsvCommand;

        public DelegateCommand EditReservationCommand
            => _editRsvCommand ?? (_editRsvCommand = new DelegateCommand(EditReservation, CanEditReservation));

        private void EditReservation()
        {
            ConditionalReservationRequest.Raise(new DataPassingNotification
            {
                Title = Resources.Register,
                Model = SelectedItem.Value.ReservationItem
            });
            UpdateRsvList();
        }

        private bool CanEditReservation() => SelectedItem.Value?.IsEditable ?? false;

        #endregion

        #region DeleteReservationCommand

        private DelegateCommand _deleteRsvCommand;

        public DelegateCommand DeleteReservationCommand
            => _deleteRsvCommand ?? (_deleteRsvCommand = new DelegateCommand(DeleteReservation, CanDeleteReservation));

        private void DeleteReservation()
        {
            SelectedItem.Value.ReservationItem.Delete();
            UpdateRsvList();
        }

        private bool CanDeleteReservation() => SelectedItem.Value != null;

        #endregion
    }
}