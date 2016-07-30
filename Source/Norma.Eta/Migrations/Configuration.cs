using System.Data.Entity.Migrations;

using Norma.Eta.Database;

using SQLite.CodeFirst;

namespace Norma.Eta.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<ReservationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            SetSqlGenerator("System.Data.SQLite.EF6", new SqliteMigrationSqlGenerator());
        }

        protected override void Seed(ReservationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}