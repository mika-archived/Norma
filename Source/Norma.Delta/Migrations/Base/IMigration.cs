namespace Norma.Delta.Migrations.Base
{
    internal interface IMigration
    {
        string MigrationId { get; }

        string UpSql();

        string DownSql();
    }
}