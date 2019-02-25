using System.Data;
using System.Data.SqlClient;

namespace Sitecore.Membership.Data.Database
{
    public abstract class BaseRepository
    {
        private readonly string _connectionString;

        protected BaseRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected IDbConnection GetConnection(bool opened = true)
        {
            var connection = new SqlConnection(_connectionString);
            if (opened)
            {
                connection.Open();
            }

            return connection;
        }
    }
}
