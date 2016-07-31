using System.Data.Common;
using System.IO;

using Norma.Eta;

namespace Norma.Delta
{
    internal static class DatabaseConnectionProvider
    {
        public static DbConnection GetConnection()
        {
            var connection = DbProviderFactories.GetFactory(NormaConstants.DatabaseProvider).CreateConnection();
            if (connection == null)
                throw new IOException();

            connection.ConnectionString = NormaConstants.DatabaseConnectionString;
            return connection;
        }
    }
}