using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IAccountDataStoreFactory _accountDataStoreFactory;

        public PaymentService(IAccountDataStoreFactory accountDataStoreFactory)
        {
            _accountDataStoreFactory = accountDataStoreFactory;
        }

        public MakePaymentResult MakePayment(MakePaymentRequest request)
        {
            var result = new MakePaymentResult { Success = false };

            var accountDataStore = _accountDataStoreFactory.BuildAccountDataStore();

            Account account = accountDataStore.GetAccount(request.DebtorAccountNumber);

            if (IsValidTransaction(account, request))
            {
                account.Balance -= request.Amount;

                accountDataStore.UpdateAccount(account);

                result.Success = true;
            }

            return result;
        }

        private bool IsValidTransaction(Account account, MakePaymentRequest request)
        {
            switch (request.PaymentScheme)
            {
                case PaymentScheme.Bacs:
                    if (account == null)
                    {
                        return false;
                    }
                    else if (!account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Bacs))
                    {
                        return false;
                    }
                    break;

                case PaymentScheme.FasterPayments:
                    if (account == null)
                    {
                        return false;
                    }
                    else if (!account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.FasterPayments))
                    {
                        return false;
                    }
                    else if (account.Balance < request.Amount)
                    {
                        return false;
                    }
                    break;

                case PaymentScheme.Chaps:
                    if (account == null)
                    {
                        return false;
                    }
                    else if (!account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Chaps))
                    {
                        return false;
                    }
                    else if (account.Status != AccountStatus.Live)
                    {
                        return false;
                    }
                    break;
            }

            return true;
        }
    }
}
