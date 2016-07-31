using System.Data.Entity;
using System.Diagnostics;

using Norma.Delta.Models;

using SQLite.CodeFirst;

namespace Norma.Delta
{
    internal class AbemaTvContext : DbContext
    {
        public DbSet<Reservation> Reservations { get; set; }

        public DbSet<KeywordReservation> KeywordReservations { get; set; }

        public DbSet<TimeReservation> TimeReservations { get; set; }

        public DbSet<SeriesReservation> SeriesReservations { get; set; }

        public DbSet<SlotReservation> SlotReservations { get; set; }

        public DbSet<Slot> Slots { get; set; }

        public DbSet<Series> Series { get; set; }

        public DbSet<Episode> Episodes { get; set; }

        public DbSet<Channel> Channels { get; set; }

        public DbSet<MigrationHistory> MigrationHistories { get; set; }

        public AbemaTvContext() : base(DatabaseConnectionProvider.GetConnection(), true)
        {
            Database.Log = log => Debug.WriteLine(log);
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