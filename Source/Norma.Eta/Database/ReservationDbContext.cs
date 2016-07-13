using System.Data.Entity;
using System.Diagnostics;

using Norma.Eta.Models.Reservations;

using SQLite.CodeFirst;

namespace Norma.Eta.Database
{
    public class ReservationDbContext : DbContext
    {
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public DbSet<RsvAll> Reservations { get; set; }

        public ReservationDbContext() : base(DatabaseConnectionProvider.GetConnection(), true)
        {
            Database.Log = str => { Debug.WriteLine(str); };
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