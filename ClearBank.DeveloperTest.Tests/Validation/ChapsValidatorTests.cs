using ClearBank.DeveloperTest.Types;
using ClearBank.DeveloperTest.Validation;
using NUnit.Framework;

namespace ClearBank.DeveloperTest.Tests.Validation
{
    [TestFixture]
    public class ChapsValidatorTests
    {
        [TestCase(AllowedPaymentSchemes.Bacs, AccountStatus.Live, false)]
        [TestCase(AllowedPaymentSchemes.Chaps, AccountStatus.Live, true)]
        [TestCase(AllowedPaymentSchemes.FasterPayments, AccountStatus.Live, false)]
        [TestCase(AllowedPaymentSchemes.Bacs, AccountStatus.Disabled, false)]
        [TestCase(AllowedPaymentSchemes.Chaps, AccountStatus.Disabled, false)]
        [TestCase(AllowedPaymentSchemes.FasterPayments, AccountStatus.Disabled, false)]
        [TestCase(AllowedPaymentSchemes.Bacs, AccountStatus.InboundPaymentsOnly, false)]
        [TestCase(AllowedPaymentSchemes.Chaps, AccountStatus.InboundPaymentsOnly, false)]
        [TestCase(AllowedPaymentSchemes.FasterPayments, AccountStatus.InboundPaymentsOnly, false)]
        public void Validates( AllowedPaymentSchemes allowedPaymentSchemes, AccountStatus status, bool expectedValidity)
        {
            // Arrange
            var account = new Account
            {
                AllowedPaymentSchemes = allowedPaymentSchemes,
                Status = status
            };

            var validator = new ChapsValidator();

            // Act
            bool isValid = validator.IsValid(account, 10);

            // Assert
            Assert.AreEqual(expectedValidity, isValid);
        }
    }
}
