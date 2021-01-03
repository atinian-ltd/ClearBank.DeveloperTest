using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Validation
{
    public class FasterPaymentsValidator : SafeValidator, IValidator
    {
        private readonly decimal _amount;

        public FasterPaymentsValidator(decimal amount)
        {
            _amount = amount;
        }

        protected override bool IsValid(Account account)
        {
            if (!account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.FasterPayments))
            {
                return false;
            }
            
            if (account.Balance < _amount)
            {
                return false;
            }

            return true;
        }
    }
}