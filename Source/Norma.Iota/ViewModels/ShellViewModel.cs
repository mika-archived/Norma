using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Input;

using Norma.Delta.Services;
using Norma.Eta.Mvvm;
using Norma.Eta.Notifications;
using Norma.Iota.Models;
using Norma.Iota.ViewModels.Controls;

using Prism.Commands;
using Prism.Interactivity.InteractionRequest;

using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace Norma.Iota.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class ShellViewModel : ViewModel
    {
        private readonly DayTable _dayTable;
        private readonly List<IDisposable> _disposables;
        private readonly SearchTable _searchTable;
        public ReadOnlyObservableCollection<ChannelCellViewModel> Channels { get; }
        public ReadOnlyObservableCollection<EpisodeCellViewModel> SearchSlots { get; }
        public ReadOnlyObservableCollection<string> AvailableDates { get; }
        public ReactiveProperty<string> SelectedDate { get; }
        public ReactiveProperty<string> SearchQuery { get; }
        public ReactiveProperty<bool> IsSearchMode { get; }
        public ReactiveCommand RunQueryCommand { get; }
        public ReactiveCommand ClearQueryCommand { get; }
        public InteractionRequest<INotification> ProgramDetailsRequest { get; }
        public InteractionRequest<INotification> ReservationListRequest { get; }

        public ShellViewModel(DatabaseService databaseService)
        {
            ProgramDetailsRequest = new InteractionRequest<INotification>();
            ReservationListRequest = new InteractionRequest<INotification>();
            SelectedDate = new ReactiveProperty<string>();
            SelectedDate.Where(w => !string.IsNullOrWhiteSpace(w)).ObserveOn(TaskPoolScheduler.Default).Subscribe(w => UpdateChannels());
            SearchQuery = new ReactiveProperty<string>();
            IsSearchMode = new ReactiveProperty<bool>();
            RunQueryCommand = SearchQuery.Select(w => IsSearchMode.Value || !string.IsNullOrWhiteSpace(w)).ToReactiveCommand();
            RunQueryCommand.Subscribe(w =>
            {
                Application.Current.Dispatcher.Invoke(() => IsLoading = true);
                _searchTable.Query(SearchQuery.Value);
                GC.Collect();
                IsLoading = false;
                IsSearchMode.Value = true;
            });
            ClearQueryCommand = IsSearchMode.ToReactiveCommand();
            ClearQueryCommand.Subscribe(w =>
            {
                SearchQuery.Value = "";
                IsSearchMode.Value = false;
            });
            _disposables = new List<IDisposable>();
            _dayTable = new DayTable(databaseService);
            _searchTable = new SearchTable(databaseService);
            AvailableDates = _dayTable.AvailableDates.ToReadOnlyReactiveCollection(w => w.ToString("d"));
            AvailableDates.ToObservable().Subscribe(w => SelectedDate.Value = AvailableDates.First());
            Channels = _dayTable.ChannelTable
                                .ToReadOnlyReactiveCollection(w => new ChannelCellViewModel(w.Date, w.Channel, w.Slots).AddTo(_disposables));
            SearchSlots = _searchTable.ResultSlots
                                      .ToReadOnlyReactiveCollection(w => new EpisodeCellViewModel(new WrapSlot(w, DateTime.MinValue)))
                                      .AddTo(this);
        }

        private void UpdateChannels()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                foreach (var disposable in _disposables)
                    disposable.Dispose();
                _disposables.Clear();
                IsLoading = true;
            });
            _dayTable.Query(DateTime.Parse(SelectedDate.Value));
            GC.Collect(); // まぁ
            IsLoading = false;
        }

        #region IsLoading

        private bool _isLoading;

        public bool IsLoading
        {
            get { return _isLoading; }
            set { SetProperty(ref _isLoading, value); }
        }

        #endregion

        #region OpenReservationListCommand

        private ICommand _openRsvListCommand;

        public ICommand OpenRsvListCommand
            => _openRsvListCommand ?? (_openRsvListCommand = new DelegateCommand(OpenRsvList));

        private void OpenRsvList() => ReservationListRequest.Raise(new DataPassingNotification());

        #endregion

        #region OnMouseDownCommand

        private ICommand _onMouseDownCommand;

        public ICommand OnMouseDownCommand =>
            _onMouseDownCommand ?? (_onMouseDownCommand = new DelegateCommand<MouseButtonEventArgs>(OnMouseDown));

        private void OnMouseDown(MouseButtonEventArgs e)
        {
            if (e.ClickCount < 2)
                return;
            var vm = ((FrameworkElement) e.Source).DataContext as EpisodeCellViewModel;
            ProgramDetailsRequest.Raise(new DataPassingNotification {Model = vm?.Model});
        }

        #endregion
    }
}