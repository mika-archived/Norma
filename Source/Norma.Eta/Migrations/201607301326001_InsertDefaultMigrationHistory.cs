namespace Norma.Eta.Migrations
{
    internal class InsertDefaultMigrationHistory : IMigration
    {
        #region Implementation of IMigration

        public string MigrationId => "201607301326001_InsertDefaultMigrationHistory";

        public string UpSql() => @"
INSERT INTO MigrationHistories(MigrationId) VALUES ('201607301326000_CreateMigrationHistoryTable');
INSERT INTO MigrationHistories(MigrationId) VALUES ('201607301326001_InsertDefaultMigrationHistory');
";

        public string DownSql() => @"
DELETE FROM MigrationHistories WHERE MigrationId = '201607301326001_InsertDefaultMigrationHistory';
DELETE FROM MigrationHistories WHERE MigrationId = '201607301326000_CreateMigrationHistoryTable';
";

        #endregion
    }
}