using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Validation
{
    public interface IValidator
    {
        bool AccountCanMakePayment(Account account);
    }
}
