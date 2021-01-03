using ClearBank.DeveloperTest.Exceptions;
using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Validation
{
    public class ValidatorFactory : IValidatorFactory
    {
        public IValidator BuildValidator(MakePaymentRequest request)
        {
            switch (request.PaymentScheme)
            {
                case PaymentScheme.Bacs:
                    return new BacsValidator();

                case PaymentScheme.FasterPayments:
                    return new FasterPaymentsValidator(request.Amount);

                case PaymentScheme.Chaps:
                    return new ChapsValidator();
            }

            throw new UnknownPaymentSchemeException($"Scheme '{request.PaymentScheme}' has not been set up");
        }
    }
}
