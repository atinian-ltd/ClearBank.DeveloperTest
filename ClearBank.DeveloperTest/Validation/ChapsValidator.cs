using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Validation
{
    public class ChapsValidator : IValidator
    {
        public bool IsValid(Account account, decimal amount)
        {
            if (account == null)
            {
                return false;
            }

            if (!account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Chaps))
            {
                return false;
            }

            if (account.Status != AccountStatus.Live)
            {
                return false;
            }

            return true;
        }
    }
}