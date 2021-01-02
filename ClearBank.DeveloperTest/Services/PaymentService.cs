using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Types;
using ClearBank.DeveloperTest.Validation;

namespace ClearBank.DeveloperTest.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IAccountDataStoreFactory _accountDataStoreFactory;
        private readonly IValidatorFactory _validatorFactory;

        public PaymentService(IAccountDataStoreFactory accountDataStoreFactory, IValidatorFactory validatorFactory)
        {
            _accountDataStoreFactory = accountDataStoreFactory;
            _validatorFactory = validatorFactory;
        }

        public MakePaymentResult MakePayment(MakePaymentRequest request)
        {
            var result = new MakePaymentResult { Success = false };

            IAccountDataStore accountDataStore = _accountDataStoreFactory.BuildAccountDataStore();

            IValidator validator = _validatorFactory.BuildValidator(request.PaymentScheme);

            Account account = accountDataStore.GetAccount(request.DebtorAccountNumber);

            if (validator.IsValid(account, request.Amount))
            {
                account.Balance -= request.Amount;

                accountDataStore.UpdateAccount(account);

                result.Success = true;
            }

            return result;
        }
    }
}
