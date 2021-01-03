using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Validation
{
    public class BacsValidator : SafeValidator, IValidator
    {
        protected override bool IsValid(Account account)
        {
            return account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Bacs);
        }
    }
}