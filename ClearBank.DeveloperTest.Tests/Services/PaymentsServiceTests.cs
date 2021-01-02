using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Types;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClearBank.DeveloperTest.Tests.Services
{
    [TestFixture]
    public class PaymentsServiceTests
    {
        [Test]
        public void MakePayment_ReturnsNotNull()
        {
            // Arrange
            var service = new PaymentService();

            // Act
            MakePaymentResult result = service.MakePayment(new MakePaymentRequest());

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
