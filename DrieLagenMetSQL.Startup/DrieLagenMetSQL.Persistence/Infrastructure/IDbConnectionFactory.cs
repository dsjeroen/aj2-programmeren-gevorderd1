using System.Data;

namespace DrieLagenMetSQL.Persistence.Infrastructure
{
    /// <summary>
    /// Contract voor het aanmaken van databaseconnecties.
    /// Repositories weten niet hoe een connectie wordt opgebouwd;
    /// Startup beslist over DB-type en connection string (injectie maakt testen eenvoudig).
    /// </summary>

    public interface IDbConnectionFactory
    {
        /// <summary>
        /// Maakt een nieuwe, nog niet geopende IDbConnection.
        /// De aanroeper opent en sluit de connectie zelf (using/Dispose).
        /// </summary>
        IDbConnection Create();
    }
}
