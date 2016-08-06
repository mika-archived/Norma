using System.Data.Entity;
using System.Diagnostics;

using Norma.Delta.Models;

using SQLite.CodeFirst;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Norma.Delta
{
    internal class AbemaTvContext : DbContext
    {
        public DbSet<Reservation> Reservations { get; set; }

        public DbSet<KeywordReservation> KeywordReservations { get; set; }

        public DbSet<TimeReservation> TimeReservations { get; set; }

        public DbSet<SeriesReservation> SeriesReservations { get; set; }

        public DbSet<SlotReservation> SlotReservations { get; set; }

        public DbSet<SlotReservation2> SlotReservations2 { get; set; }

        public DbSet<Slot> Slots { get; set; }

        public DbSet<Series> Series { get; set; }

        public DbSet<Episode> Episodes { get; set; }

        public DbSet<Cast> Casts { get; set; }

        public DbSet<Copyright> Copyrights { get; set; }

        public DbSet<Crew> Crews { get; set; }

        public DbSet<Channel> Channels { get; set; }

        public DbSet<MigrationHistory> MigrationHistories { get; set; }

        public DbSet<Metadata> Metadata { get; set; }

        public AbemaTvContext() : base(DatabaseConnectionProvider.GetConnection(), true)
        {
            Database.Log = log => Debug.WriteLine(log);
            Configuration.AutoDetectChangesEnabled = false;
        }

        public void DetectChanges()
        {
            ChangeTracker.DetectChanges();
        }

        public void TurnOffLazyLoading()
        {
            Configuration.ProxyCreationEnabled = false;
        }

        #region Overrides of DbContext

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var sqliteInitiaizer = new SqliteCreateDatabaseIfNotExists<AbemaTvContext>(modelBuilder);
            Database.SetInitializer(sqliteInitiaizer);
        }

        #endregion
    }
}