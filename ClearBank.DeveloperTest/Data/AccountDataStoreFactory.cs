using ClearBank.DeveloperTest.Configuration;

namespace ClearBank.DeveloperTest.Data
{
    public class AccountDataStoreFactory : IAccountDataStoreFactory
    {
        private readonly IConfigurationProvider _configurationProvider;
        private readonly IAccountDataStore _accountDataStore;
        private readonly IAccountDataStore _backupAccountDataStore;

        public AccountDataStoreFactory(
            IConfigurationProvider configurationProvider, 
            IAccountDataStore accountDataStore, 
            IAccountDataStore backupAccountDataStore)
        {
            _configurationProvider = configurationProvider;
            _accountDataStore = accountDataStore;
            _backupAccountDataStore = backupAccountDataStore;
        }

        public IAccountDataStore BuildAccountDataStore()
        {
            _configurationProvider.TryGetDataStoreType(out string dataStoreType);

            if (dataStoreType == "Backup")
            {
                return _backupAccountDataStore;
            }
            else
            {
                return _accountDataStore;
            }
        }
    }
}
