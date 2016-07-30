namespace Norma.Eta.Migrations
{
    internal class CreateMigrationHistoryTable : IMigration
    {
        #region Implementation of IMigration

        public string MigrationId => "201607301326000_CreateMigrationHistoryTable";

        public string UpSql() => @"
CREATE TABLE MigrationHistories(MigrationId NVARCHAR(150) PRIMARY KEY);
";

        public string DownSql() => "DROP TABLE IF EXISTS MigrationHistories";

        #endregion
    }
}