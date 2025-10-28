using Microsoft.Data.SqlClient;
using System.Data;

namespace DrieLagenMetSQL.Persistence.Infrastructure
{
    /// <summary>
    /// Concrete fabriek die SQL Server connecties aanmaakt op basis van één centrale connection string.
    /// 
    /// Voordelen:
    /// - Eén bron van waarheid voor de connection string (config/Startup).
    /// - Repositories blijven los van Microsoft.Data.SqlClient (programmeer tegen IDbConnection).
    /// - Eenvoudig te vervangen door bv. SQLiteConnectionFactory, Test/FakeFactory, …
    /// </summary>

    public sealed class SqlConnectionFactory :IDbConnectionFactory
    {
        private readonly string _connectionString;

        /// <summary>
        /// Geef de connection string aan via Startup/config.
        /// </summary>
        public SqlConnectionFactory(string connectionString)
        {
            _connectionString = connectionString 
                ?? throw new ArgumentNullException(nameof(connectionString));
        }

        /// <summary>
        /// Maakt een *gesloten* SqlConnection. De aanroeper opent/Dispose't.
        /// </summary>
        public IDbConnection Create() => new SqlConnection(_connectionString);
    }
}
