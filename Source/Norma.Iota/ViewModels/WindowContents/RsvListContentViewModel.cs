using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

using Norma.Eta.Models;
using Norma.Eta.Mvvm;
using Norma.Eta.Notifications;
using Norma.Eta.Properties;
using Norma.Iota.ViewModels.Reservations;

using Prism.Commands;

using Reactive.Bindings;

namespace Norma.Iota.ViewModels.WindowContents
{
    internal class RsvListContentViewModel : InteractionViewModel<DataPassingNotification>
    {
        private readonly Reservation _reservation;
        public string WindowTitle => Resources.RsvList;

        public ObservableCollection<RsvAllViewModel> Reservations { get; }
        public ReactiveProperty<RsvAllViewModel> SelectedItem { get; }

        public RsvListContentViewModel(Reservation reservation)
        {
            _reservation = reservation;
            Reservations = new ObservableCollection<RsvAllViewModel>();
            SelectedItem = new ReactiveProperty<RsvAllViewModel>();
            SelectedItem.Subscribe(w =>
            {
                ((DelegateCommand) EditReservationCommand).RaiseCanExecuteChanged();
                ((DelegateCommand) DeleteReservationCommand).RaiseCanExecuteChanged();
            }).AddTo(this);

            ViewModelHelper.Subscribe(this, nameof(Notification), w => UpdateRsvList());
        }

        private void UpdateRsvList()
        {
            Reservations.Clear();
            foreach (var rsv in _reservation.Reservations.Where(x => x.IsEnable))
                Reservations.Add(new RsvAllViewModel(rsv));
        }

        #region EditReservationCommand

        private ICommand _editRsvCommand;

        public ICommand EditReservationCommand
            => _editRsvCommand ?? (_editRsvCommand = new DelegateCommand(EditReservation, CanEditReservation));

        private void EditReservation()
        {
            UpdateRsvList();
        }

        private bool CanEditReservation() => SelectedItem.Value != null;

        #endregion

        #region DeleteReservationCommand

        private ICommand _deleteRsvCommand;

        public ICommand DeleteReservationCommand
            => _deleteRsvCommand ?? (_deleteRsvCommand = new DelegateCommand(DeleteReservation, CanDeleteReservation));

        private void DeleteReservation()
        {
            // ここで確認ダイアログを出したい。
            _reservation.Reservations.Single(w => w.Id == SelectedItem.Value.Model.Id).IsEnable = false;
            UpdateRsvList();
        }

        private bool CanDeleteReservation() => SelectedItem.Value != null;

        #endregion
    }
}