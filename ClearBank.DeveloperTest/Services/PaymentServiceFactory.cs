using ClearBank.DeveloperTest.Configuration;
using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Validation;

namespace ClearBank.DeveloperTest.Services
{
    public class PaymentServiceFactory : IPaymentServiceFactory
    {
        public IPaymentService BuildPaymentService()
        {
            var configurationProvider = new ConfigurationProvider();

            var accountDataStoreFactory = new AccountDataStoreFactory(configurationProvider);

            var validatorFactory = new ValidatorFactory();

            return new PaymentService(accountDataStoreFactory, validatorFactory);
        }
    }
}
