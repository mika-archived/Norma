using System.Collections.ObjectModel;
using System.Data.Entity.Infrastructure;

using Norma.Delta.Models;

namespace Norma.Delta.Services
{
    public class TimetableService
    {
        private readonly ObservableCollection<Slot> _currentSlotsInternal;
        private readonly DatabaseService _databaseService;

        public DbQuery<Slot> Slots => _databaseService.Slots.AsNoTracking();

        public DbQuery<Episode> Episodes => _databaseService.Episodes.AsNoTracking();

        public DbQuery<Series> Series => _databaseService.Series.AsNoTracking();

        public DbQuery<Channel> Channels => _databaseService.Channels.AsNoTracking();

        public TimetableService(DatabaseService databaseService)
        {
            _databaseService = databaseService;
            _currentSlotsInternal = new ObservableCollection<Slot>();
        }

        #region CurrentSlots

        private ReadOnlyObservableCollection<Slot> _currentSlots;

        public ReadOnlyObservableCollection<Slot> CurrentSlots
            => _currentSlots ?? (_currentSlots = new ReadOnlyObservableCollection<Slot>(_currentSlotsInternal));

        #endregion
    }
}