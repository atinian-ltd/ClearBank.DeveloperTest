using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Validation
{
    public class FasterPaymentsValidator : IValidator
    {
        public bool IsValid(Account account, decimal amount)
        {
            if (account == null)
            {
                return false;
            }
            
            if (!account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.FasterPayments))
            {
                return false;
            }
            
            if (account.Balance < amount)
            {
                return false;
            }

            return true;
        }
    }
}