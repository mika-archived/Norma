using System.Windows.Input;

using Norma.Delta.Services;
using Norma.Eta.Mvvm;
using Norma.Eta.Notifications;
using Norma.Eta.Properties;

using Prism.Commands;
using Prism.Interactivity.InteractionRequest;

namespace Norma.Iota.ViewModels.WindowContents
{
    internal class RsvListContentViewModel : InteractionViewModel<DataPassingNotification>
    {
        private readonly DatabaseService _databaseService;
        public string WindowTitle => Resources.RsvList;

        //public ObservableCollection<RsvAllViewModel> Reservations { get; }
        //public ReactiveProperty<RsvAllViewModel> SelectedItem { get; }
        public InteractionRequest<Confirmation> ConfirmationRequest { get; }

        public InteractionRequest<DataPassingNotification> EditRequest { get; }

        public RsvListContentViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
            //Reservations = new ObservableCollection<RsvAllViewModel>();
            //SelectedItem = new ReactiveProperty<RsvAllViewModel>();
            ConfirmationRequest = new InteractionRequest<Confirmation>();
            EditRequest = new InteractionRequest<DataPassingNotification>();
            /*
            SelectedItem.Subscribe(w =>
            {
                ((DelegateCommand) EditReservationCommand).RaiseCanExecuteChanged();
                ((DelegateCommand) DeleteReservationCommand).RaiseCanExecuteChanged();
            }).AddTo(this);
            */
            ViewModelHelper.Subscribe(this, w => w.Notification, w => UpdateRsvList());
        }

        private void UpdateRsvList()
        {
            //Reservations.Clear();
            /*
            foreach (var rsv in _reservation.Reservations.Where(x => x.IsEnable))
                Reservations.Add(new RsvAllViewModel(rsv));
            */
        }

        #region EditReservationCommand

        private ICommand _editRsvCommand;

        public ICommand EditReservationCommand
            => _editRsvCommand ?? (_editRsvCommand = new DelegateCommand(EditReservation, CanEditReservation));

        private async void EditReservation()
        {
            //await EditRequest.RaiseAsync(new DataPassingNotification {Model = SelectedItem.Value.Model});
            //UpdateRsvList();
        }

        private bool CanEditReservation() => false; //SelectedItem.Value != null && SelectedItem.Value.Type != nameof(RsvProgram)

        //&& SelectedItem.Value.Type != nameof(RsvProgram2);

        #endregion

        #region DeleteReservationCommand

        private ICommand _deleteRsvCommand;

        public ICommand DeleteReservationCommand
            => _deleteRsvCommand ?? (_deleteRsvCommand = new DelegateCommand(DeleteReservation, CanDeleteReservation));

        private async void DeleteReservation()
        {
            // ここで確認ダイアログを出したい。
            var result = await ConfirmationRequest.RaiseAsync(new Confirmation
            {
                Title = Resources.Confirmation,
                Content = Resources.ConfirmDelete
            });
            if (!result.Confirmed)
                return;
            //_reservation.DeleteReservation(SelectedItem.Value.Model);
            UpdateRsvList();
        }

        private bool CanDeleteReservation() => false; //SelectedItem.Value != null;

        #endregion
    }
}