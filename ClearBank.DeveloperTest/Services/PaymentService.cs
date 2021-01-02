using ClearBank.DeveloperTest.Configuration;
using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Types;
using System;

namespace ClearBank.DeveloperTest.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfigurationProvider _configurationProvider;
        private readonly IAccountDataStore _accountDataStore;
        private readonly IAccountDataStore _backupAccountDataStore;

        public PaymentService(
            IConfigurationProvider configurationProvider,
            IAccountDataStore accountDataStore,
            IAccountDataStore backupAccountDataStore)
        {
            _configurationProvider = configurationProvider;
            _accountDataStore = accountDataStore;
            _backupAccountDataStore = backupAccountDataStore;
        }

        public MakePaymentResult MakePayment(MakePaymentRequest request)
        {
            var result = new MakePaymentResult { Success = false };

            Account account = GetAccount(request.DebtorAccountNumber);

            if (IsValidTransaction(account, request))
            {
                account.Balance -= request.Amount;

                UpdateAccount(account);

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

        private void UpdateAccount(Account account)
        {
            _configurationProvider.TryGetDataStoreType(out string dataStoreType);

            if (dataStoreType == "Backup")
            {
                _backupAccountDataStore.UpdateAccount(account);
            }
            else
            {
                _accountDataStore.UpdateAccount(account);
            }
        }

        private Account GetAccount(string accountNumber)
        {
            _configurationProvider.TryGetDataStoreType(out string dataStoreType);

            if (dataStoreType == "Backup")
            {
                return _backupAccountDataStore.GetAccount(accountNumber);
            }
            else
            {
                return _accountDataStore.GetAccount(accountNumber);
            }
        }
    }
}
