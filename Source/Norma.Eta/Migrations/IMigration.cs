namespace Norma.Eta.Migrations
{
    internal interface IMigration
    {
        string MigrationId { get; }

        string UpSql();

        string DownSql();
    }
}