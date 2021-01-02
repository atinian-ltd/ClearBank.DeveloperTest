using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Validation
{
    public class BacsValidator : IValidator
    {
        public bool IsValid(Account account, decimal amount)
        {
            if (account == null)
            {
                return false;
            }

            return account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Bacs);
        }
    }
}