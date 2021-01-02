using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Validation
{
    public interface IValidator
    {
        bool IsValid(Account account, decimal amount);
    }
}
