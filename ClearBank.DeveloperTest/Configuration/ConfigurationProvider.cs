using System.Configuration;

namespace ClearBank.DeveloperTest.Configuration
{
    public class ConfigurationProvider : IConfigurationProvider
    {
        public bool TryGetDataStoreType(out string dataStoreType)
        {
            // AppSettings indexer returns null if setting key does not exist
            dataStoreType = ConfigurationManager.AppSettings["DataStoreType"];

            return string.IsNullOrWhiteSpace(dataStoreType);
        }
    }
}