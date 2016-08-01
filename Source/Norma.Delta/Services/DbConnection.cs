using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

using Norma.Delta.Extensions;
using Norma.Delta.Migrations.Base;
using Norma.Delta.Models;

namespace Norma.Delta.Services
{
    public class DbConnection : IDisposable
    {
        private readonly DatabaseService _databaseService;
        private readonly AbemaTvContext _dbContext;
        private readonly object _lockObj = new object();

        private readonly List<IMigration> _migrations = new List<IMigration>();

        public DbSet<Reservation> Reservations => _dbContext.Reservations;
        public DbSet<KeywordReservation> KeywordReservations => _dbContext.KeywordReservations;
        public DbSet<TimeReservation> TimeReservations => _dbContext.TimeReservations;
        public DbSet<SeriesReservation> SeriesReservations => _dbContext.SeriesReservations;
        public DbSet<SlotReservation> SlotReservations => _dbContext.SlotReservations;
        public DbSet<SlotReservation2> SlotReservations2 => _dbContext.SlotReservations2;
        public DbSet<Slot> Slots => _dbContext.Slots;
        public DbSet<Series> Series => _dbContext.Series;
        public DbSet<Episode> Episodes => _dbContext.Episodes;
        public DbSet<Channel> Channels => _dbContext.Channels;
        public DbSet<Cast> Casts => _dbContext.Casts;
        public DbSet<Copyright> Copyrights => _dbContext.Copyrights;
        public DbSet<Crew> Crews => _dbContext.Crews;
        public DbSet<Metadata> Metadata => _dbContext.Metadata;

        internal DbConnection(DatabaseService databaseService)
        {
            _databaseService = databaseService;
            _dbContext = new AbemaTvContext();
        }

        #region Implementation of IDisposable

        public void Dispose()
        {
            _dbContext.Dispose();
            _databaseService.Disconnect();
        }

        #endregion

        public void Initialize()
        {
            lock (_lockObj)
            {
                _dbContext.MigrationHistories.Create();
                _dbContext.Episodes.Create();
                _dbContext.Channels.Create();
                _dbContext.Slots.Create();
                _dbContext.Series.Create();
                _dbContext.Reservations.Create();
                _dbContext.KeywordReservations.Create();
                _dbContext.TimeReservations.Create();
                _dbContext.SeriesReservations.Create();
                _dbContext.SlotReservations.Create();
                _dbContext.SaveChanges();
            }
        }

        public void Migration()
        {
            lock (_lockObj)
            {
                var lastMigration = _dbContext.MigrationHistories.OrderByDescending(w => w.MigrationHistoryId).FirstOrDefault();
                var migrationTargets = lastMigration == null
                    ? _migrations
                    : _migrations.SkipWhile(w => w.MigrationId != lastMigration.MigrationHistoryId).Skip(1);
                var migrations = migrationTargets as IMigration[] ?? migrationTargets.ToArray();
                if (!migrations.Any())
                    return;

                foreach (var migration in migrations)
                {
                    _dbContext.Database.ExecuteSqlCommand(migration.UpSql().Replace(Environment.NewLine, ""));
                    _dbContext.MigrationHistories.Add(new MigrationHistory {MigrationHistoryId = migration.MigrationId});
                }
                _dbContext.SaveChanges();
            }
        }

        public void Cleanup()
        {
            lock (_lockObj)
            {
                // 無効化されているもの
                foreach (var reservations in _dbContext.Reservations.Where(w => !w.IsEnabled))
                {
                    _dbContext.Reservations.Remove(reservations);
                    _dbContext.KeywordReservations.RemoveIfExists(reservations.KeywordReservation);
                    _dbContext.TimeReservations.RemoveIfExists(reservations.TimeReservation);
                    _dbContext.SeriesReservations.RemoveIfExists(reservations.SeriesReservation);
                    _dbContext.SlotReservations.RemoveIfExists(reservations.SlotReservation);
                }

                // 放送済み
                foreach (var slot in _dbContext.Slots.Where(w => w.StartAt <= DateTime.Now))
                {
                    _dbContext.Slots.Remove(slot);
                    foreach (var episode in slot.Episodes)
                        _dbContext.Episodes.Remove(episode);
                }

                _dbContext.SaveChanges();
            }
        }

        public void FullCleanup()
        {
            Cleanup();

            lock (_lockObj)
            {
                // 参照されていない番組
                foreach (var episode in _dbContext.Episodes.Where(w => !w.Slots.Any()))
                    _dbContext.Episodes.Remove(episode);
            }
        }

        public void SaveChanges()
        {
            lock (_lockObj)
                _dbContext.SaveChanges();
        }

        public void DetechChanges()
        {
            lock (_lockObj)
                _dbContext.DetectChanges();
        }
    }
}