using System.Data;

namespace DrieLagenMetSQL.Persistence.Infrastructure
{
    /// <summary>
    /// Contract voor het aanmaken van databaseconnecties.
    /// 
    /// Doel:
    /// - Repositories hoeven niet te weten hoe een connectie wordt gemaakt.
    /// - Startup bepaalt welk DB-type en welke connection string gebruikt wordt.
    /// - Maakt testen/mocken eenvoudig (fake factory injection).
    /// 
    /// Gebruik in de flow:
    /// Presentation → DomainController → IRepository<T> → (Persistence)
    ///                                               ↘ IDbConnectionFactory.Create() → IDbConnection → SQL
    /// </summary>

    public interface IDbConnectionFactory
    {
        /// <summary>
        /// Maak een nieuwe, nog niet geopende, IDbConnection.
        /// De aanroeper (repo) opent/weggooit de connectie (using/Dispose).
        /// </summary>
        IDbConnection Create();
    }
}
