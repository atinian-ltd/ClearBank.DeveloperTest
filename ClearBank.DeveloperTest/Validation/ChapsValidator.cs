using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Validation
{
    public class ChapsValidator : SafeValidator, IValidator
    {
        protected override bool IsValid(Account account)
        {
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