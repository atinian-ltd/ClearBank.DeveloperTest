using ClearBank.DeveloperTest.Exceptions;
using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Validation
{
    public class ValidatorFactory : IValidatorFactory
    {
        public IValidator BuildValidator(PaymentScheme scheme)
        {
            switch (scheme)
            {
                case PaymentScheme.Bacs:
                    return new BacsValidator();

                case PaymentScheme.FasterPayments:
                    return new FasterPaymentsValidator();

                case PaymentScheme.Chaps:
                    return new ChapsValidator();
            }

            throw new UnknownPaymentSchemeException($"Scheme '{scheme}' has not been set up");
        }
    }
}
