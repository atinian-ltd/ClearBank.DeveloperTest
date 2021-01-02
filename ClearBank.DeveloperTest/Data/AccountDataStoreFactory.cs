using ClearBank.DeveloperTest.Configuration;

namespace ClearBank.DeveloperTest.Data
{
    public class AccountDataStoreFactory : IAccountDataStoreFactory
    {
        private readonly IConfigurationProvider _configurationProvider;

        public AccountDataStoreFactory(IConfigurationProvider configurationProvider)
        {
            _configurationProvider = configurationProvider;
        }

        public IAccountDataStore BuildAccountDataStore()
        {
            _configurationProvider.TryGetDataStoreType(out string dataStoreType);

            if (dataStoreType == "Backup")
            {
                return new BackupAccountDataStore();
            }
            else
            {
                return new AccountDataStore();
            }
        }
    }
}
