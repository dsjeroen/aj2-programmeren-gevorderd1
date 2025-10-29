using Microsoft.Data.SqlClient;
using System.Data;

namespace DrieLagenMetSQL.Persistence.Infrastructure
{
    /// <summary>
    /// Concrete fabriek die SQL Server-connecties aanmaakt op basis van één centrale connection string.
    /// Eén bron van waarheid voor de string; eenvoudig te vervangen door andere factory (SQLite, Fake, …).
    /// Sealed class: bedoeld als definitieve infrastructuurimplementatie, niet uitbreidbaar.
    /// </summary>

    public sealed class SqlConnectionFactory :IDbConnectionFactory
    {
        private readonly string _connectionString;

        /// <summary>Ontvangt de connection string via Startup of configuratie.</summary>
        public SqlConnectionFactory(string connectionString)
        {
            _connectionString = connectionString
                ?? throw new ArgumentNullException(nameof(connectionString));
        }

        /// <summary>Maakt een gesloten SqlConnection; aanroeper opent en sluit zelf (using/Dispose).</summary>
        public IDbConnection Create() => new SqlConnection(_connectionString);
    }
}
