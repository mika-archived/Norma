using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Diagnostics;
using System.Linq;

using Norma.Eta.Migrations;
using Norma.Eta.Models;
using Norma.Eta.Models.Reservations;

using SQLite.CodeFirst;

namespace Norma.Eta.Database
{
    public class ReservationDbContext : DbContext
    {
        private readonly List<IMigration> _migrations = new List<IMigration>
        {
            new CreateMigrationHistoryTable(),
            new InsertDefaultMigrationHistory(),
            new AddRsvAllSeriesId()
        };

        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public DbSet<RsvAll> Reservations { get; set; }

        public DbSet<MigrationHistory> MigrationHistories { get; set; }

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

        public void Migrate()
        {
            try
            {
                var test = Reservations.SingleOrDefault(w => w.Id == 1);
            }
            catch (EntityCommandExecutionException)
            {
                try
                {
                    var test = MigrationHistories.FirstOrDefault();
                }
                catch (EntityCommandExecutionException)
                {
                    CreateMigrationHistory();
                    SaveChanges();
                }

                // Migration required
                var lastMigration = MigrationHistories.OrderByDescending(w => w.MigrationId).FirstOrDefault();
                var migrationTargets = lastMigration == null
                    ? _migrations
                    : _migrations.SkipWhile(w => w.MigrationId != lastMigration.MigrationId).Skip(1);

                var migrations = migrationTargets as IMigration[] ?? migrationTargets.ToArray();
                try
                {
                    foreach (var migration in migrations)
                    {
                        Database.ExecuteSqlCommand(migration.UpSql().Replace(Environment.NewLine, ""));
                        MigrationHistories.Add(new MigrationHistory {MigrationId = migration.MigrationId});
                    }
                    SaveChanges();
                }
                catch (Exception e)
                {
                    foreach (var migration in migrations.Reverse())
                    {
                        Database.ExecuteSqlCommand(migration.DownSql().Replace(Environment.NewLine, ""));
                        MigrationHistories.Remove(new MigrationHistory {MigrationId = migration.MigrationId});
                    }
                    SaveChanges();
                }
            }
        }

        private void CreateMigrationHistory()
        {
            var firstMigration = _migrations.First();
            var secondMigration = _migrations.Skip(1).First();
            try
            {
                Database.ExecuteSqlCommand(firstMigration.UpSql().Replace(Environment.NewLine, ""));
                Database.ExecuteSqlCommand(secondMigration.UpSql().Replace(Environment.NewLine, ""));
            }
            catch (Exception)
            {
                // 失敗したらどうしよう。
                Database.ExecuteSqlCommand(secondMigration.DownSql().Replace(Environment.NewLine, ""));
                Database.ExecuteSqlCommand(firstMigration.DownSql().Replace(Environment.NewLine, ""));
            }
        }
    }
}