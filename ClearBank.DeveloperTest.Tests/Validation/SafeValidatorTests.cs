using ClearBank.DeveloperTest.Types;
using ClearBank.DeveloperTest.Validation;
using NUnit.Framework;
using System;

namespace ClearBank.DeveloperTest.Tests.Validation
{
    [TestFixture]
    public class SafeValidatorTests
    {
        [Test]
        public void ThrowsOnNullAccount()
        {
            // Arrange
            var stub = new StubValidator();

            // Act / Assert
            Assert.Throws<ArgumentNullException>(() => stub.AccountCanMakePayment(null));
        }

        class StubValidator : SafeValidator
        {
            protected override bool IsValid(Account account)
            {
                return true;
            }
        }
    }
}
