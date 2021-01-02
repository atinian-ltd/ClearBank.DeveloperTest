using ClearBank.DeveloperTest.Types;
using ClearBank.DeveloperTest.Validation;
using NUnit.Framework;
using System;

namespace ClearBank.DeveloperTest.Tests.Validation
{
    [TestFixture]
    public class ValidatorFactoryTests
    {
        [TestCase(PaymentScheme.Bacs, typeof(BacsValidator))]
        [TestCase(PaymentScheme.Chaps, typeof(ChapsValidator))]
        [TestCase(PaymentScheme.FasterPayments, typeof(FasterPaymentsValidator))]
        public void ReturnsCorrectValidator(PaymentScheme scheme, Type expectedType)
        {
            // Arrange
            var factory = new ValidatorFactory();

            // Act
            var validator = factory.BuildValidator(scheme);

            // Assert
            Assert.AreEqual(expectedType, validator.GetType());
        }
    }
}
