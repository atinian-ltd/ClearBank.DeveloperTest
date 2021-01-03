using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Validation
{
    public interface IValidatorFactory
    {
        IValidator BuildValidator(MakePaymentRequest request);
    }
}