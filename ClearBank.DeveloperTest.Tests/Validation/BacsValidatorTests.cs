using ClearBank.DeveloperTest.Types;
using ClearBank.DeveloperTest.Validation;
using NUnit.Framework;

namespace ClearBank.DeveloperTest.Tests.Validation
{
    [TestFixture]
    public class BacsValidatorTests
    {
        [TestCase(AllowedPaymentSchemes.Bacs, true)]
        [TestCase(AllowedPaymentSchemes.Chaps, false)]
        [TestCase(AllowedPaymentSchemes.FasterPayments, false)]
        public void Validates(AllowedPaymentSchemes allowedPaymentSchemes, bool expectedValidity)
        {
            // Arrange
            var account = new Account
            {
                AllowedPaymentSchemes = allowedPaymentSchemes
            };

            var validator = new BacsValidator();

            // Act
            bool isValid = validator.AccountCanMakePayment(account);

            // Assert
            Assert.AreEqual(expectedValidity, isValid);
        }
    }
}
