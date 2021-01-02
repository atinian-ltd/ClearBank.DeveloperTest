using ClearBank.DeveloperTest.Types;
using ClearBank.DeveloperTest.Validation;
using NUnit.Framework;

namespace ClearBank.DeveloperTest.Tests.Validation
{
    [TestFixture]
    public class FasterPaymentsValidatorTests
    {
        [TestCase(AllowedPaymentSchemes.FasterPayments, 100, 10, true)]
        [TestCase(AllowedPaymentSchemes.Chaps, 100, 10, false)]
        [TestCase(AllowedPaymentSchemes.Bacs, 100, 10, false)]
        [TestCase(AllowedPaymentSchemes.FasterPayments, 1, 10, false)]
        public void Validates(AllowedPaymentSchemes allowedPaymentSchemes, decimal balance, decimal amount, bool expectedValidity)
        {
            // Arrange
            var account = new Account
            {
                AllowedPaymentSchemes = allowedPaymentSchemes,
                Balance = balance
            };

            var validator = new FasterPaymentsValidator();

            // Act
            bool isValid = validator.IsValid(account, amount);

            // Assert
            Assert.AreEqual(expectedValidity, isValid);
        }
    }
}
