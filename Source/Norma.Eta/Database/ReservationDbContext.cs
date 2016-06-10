using System.Data.Entity;

using Norma.Eta.Models.Reservations;

using SQLite.CodeFirst;

namespace Norma.Eta.Database
{
    public class ReservationDbContext : DbContext
    {
        public DbSet<RsvAll> Reservations { get; set; }

        public ReservationDbContext() : base(DatabaseConnectionProvider.GetConnection(), true)
        {

        }

        #region Overrides of DbContext

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var sqliteInitiaizer = new SqliteCreateDatabaseIfNotExists<ReservationDbContext>(modelBuilder);
            System.Data.Entity.Database.SetInitializer(sqliteInitiaizer);
        }

        #endregion
    }
}