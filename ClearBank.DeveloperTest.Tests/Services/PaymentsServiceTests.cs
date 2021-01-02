using ClearBank.DeveloperTest.Configuration;
using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Types;
using Moq;
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
            var configuration = new Mock<IConfigurationProvider>();
            var accountDataStore = new Mock<IAccountDataStore>();
            var backupAccountDataStore = new Mock<IAccountDataStore>();
            var service = new PaymentService(configuration.Object, accountDataStore.Object, backupAccountDataStore.Object);

            // Act
            MakePaymentResult result = service.MakePayment(new MakePaymentRequest());

            // Assert
            Assert.IsNotNull(result);
        }

    }
}
