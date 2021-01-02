namespace ClearBank.DeveloperTest.Configuration
{
    public interface IConfigurationProvider
    {
        bool TryGetDataStoreType(out string dataStoreType);
    }
}