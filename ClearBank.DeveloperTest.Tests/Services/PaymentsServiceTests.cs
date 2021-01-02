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

        [TestCase(PaymentScheme.Bacs)]
        [TestCase(PaymentScheme.Chaps)]
        [TestCase(PaymentScheme.FasterPayments)]
        public void MakePayment_ReturnsFailureIfAccountNotFound(PaymentScheme paymentScheme)
        {
            // Arrange
            var configuration = new Mock<IConfigurationProvider>();
            var accountDataStore = new Mock<IAccountDataStore>();
            accountDataStore.Setup(ds => ds.GetAccount(It.IsAny<string>())).Returns((Account) null);

            var backupAccountDataStore = new Mock<IAccountDataStore>();
            var service = new PaymentService(configuration.Object, accountDataStore.Object, backupAccountDataStore.Object);

            var request = new MakePaymentRequest
            {
                PaymentScheme = paymentScheme
            };

            // Act
            MakePaymentResult result = service.MakePayment(new MakePaymentRequest());

            // Assert
            Assert.IsFalse(result.Success);
            accountDataStore.Verify(ds => ds.UpdateAccount(It.IsAny<Account>()), Times.Never);
            backupAccountDataStore.Verify(ds => ds.UpdateAccount(It.IsAny<Account>()), Times.Never);
        }

        [TestCase(AllowedPaymentSchemes.Bacs, PaymentScheme.Chaps)]
        [TestCase(AllowedPaymentSchemes.FasterPayments, PaymentScheme.Chaps)]
        [TestCase(AllowedPaymentSchemes.FasterPayments | AllowedPaymentSchemes.Bacs, PaymentScheme.Chaps)]
        [TestCase(AllowedPaymentSchemes.Bacs, PaymentScheme.FasterPayments)]
        [TestCase(AllowedPaymentSchemes.Chaps, PaymentScheme.FasterPayments)]
        [TestCase(AllowedPaymentSchemes.Bacs | AllowedPaymentSchemes.Chaps, PaymentScheme.FasterPayments)]
        [TestCase(AllowedPaymentSchemes.Chaps, PaymentScheme.Bacs)]
        [TestCase(AllowedPaymentSchemes.FasterPayments, PaymentScheme.Bacs)]
        [TestCase(AllowedPaymentSchemes.FasterPayments | AllowedPaymentSchemes.Chaps, PaymentScheme.Bacs)]
        public void MakePayment_ReturnsFailureIfSchemeNotAllowed(AllowedPaymentSchemes allowedSchemes, PaymentScheme requestScheme)
        {
            // Arrange
            var account = new Account
            {
                AllowedPaymentSchemes = allowedSchemes
            };

            var configuration = new Mock<IConfigurationProvider>();
            var accountDataStore = new Mock<IAccountDataStore>();
            accountDataStore.Setup(ds => ds.GetAccount(It.IsAny<string>())).Returns(account);

            var backupAccountDataStore = new Mock<IAccountDataStore>();
            var service = new PaymentService(configuration.Object, accountDataStore.Object, backupAccountDataStore.Object);

            var request = new MakePaymentRequest
            {
                PaymentScheme = requestScheme
            };

            // Act
            MakePaymentResult result = service.MakePayment(request);

            // Assert
            Assert.IsFalse(result.Success);
            accountDataStore.Verify(ds => ds.UpdateAccount(It.IsAny<Account>()), Times.Never);
            backupAccountDataStore.Verify(ds => ds.UpdateAccount(It.IsAny<Account>()), Times.Never);
        }

        [Test]
        public void MakePayment_FasterPayment_ReturnsFailureIfInsufficientFunds()
        {
            // Arrange
            var account = new Account
            {
                AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments,
                Balance = 1
            };

            var configuration = new Mock<IConfigurationProvider>();
            var accountDataStore = new Mock<IAccountDataStore>();
            accountDataStore.Setup(ds => ds.GetAccount(It.IsAny<string>())).Returns(account);

            var backupAccountDataStore = new Mock<IAccountDataStore>();
            var service = new PaymentService(configuration.Object, accountDataStore.Object, backupAccountDataStore.Object);

            var request = new MakePaymentRequest
            {
                PaymentScheme = PaymentScheme.FasterPayments,
                Amount = 2
            };

            // Act
            MakePaymentResult result = service.MakePayment(request);

            // Assert
            Assert.IsFalse(result.Success);
            accountDataStore.Verify(ds => ds.UpdateAccount(It.IsAny<Account>()), Times.Never);
            backupAccountDataStore.Verify(ds => ds.UpdateAccount(It.IsAny<Account>()), Times.Never);
        }

        [TestCase(AccountStatus.Disabled)]
        [TestCase(AccountStatus.InboundPaymentsOnly)]
        public void MakePayment_Chaps_ReturnsFailureIfInactive(AccountStatus status)
        {
            // Arrange
            var account = new Account
            {
                AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps,
                Status = status
            };

            var configuration = new Mock<IConfigurationProvider>();
            var accountDataStore = new Mock<IAccountDataStore>();
            accountDataStore.Setup(ds => ds.GetAccount(It.IsAny<string>())).Returns(account);

            var backupAccountDataStore = new Mock<IAccountDataStore>();
            var service = new PaymentService(configuration.Object, accountDataStore.Object, backupAccountDataStore.Object);

            var request = new MakePaymentRequest
            {
                PaymentScheme = PaymentScheme.Chaps
            };

            // Act
            MakePaymentResult result = service.MakePayment(request);

            // Assert
            Assert.IsFalse(result.Success);
            accountDataStore.Verify(ds => ds.UpdateAccount(It.IsAny<Account>()), Times.Never);
            backupAccountDataStore.Verify(ds => ds.UpdateAccount(It.IsAny<Account>()), Times.Never);
        }

        [TestCase("Live")]
        [TestCase("")]
        [TestCase(null)]
        public void MakePayment_DefaultsToLiveDataStore(string settingValue)
        {
            // Arrange
            var configuration = new Mock<IConfigurationProvider>();
            configuration.Setup(c => c.TryGetDataStoreType(out settingValue)).Returns(true);

            var accountDataStore = new Mock<IAccountDataStore>();
            var backupAccountDataStore = new Mock<IAccountDataStore>();
            var service = new PaymentService(configuration.Object, accountDataStore.Object, backupAccountDataStore.Object);

            // Act
            MakePaymentResult result = service.MakePayment(new MakePaymentRequest());

            // Assert
            accountDataStore.Verify(ds => ds.GetAccount(It.IsAny<string>()), Times.Once);
            backupAccountDataStore.Verify(ds => ds.GetAccount(It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void MakePayment_UsesBackupDataStoreWhenConfigured()
        {
            // Arrange
            string settingValue = "Backup";

            var configuration = new Mock<IConfigurationProvider>();
            configuration.Setup(c => c.TryGetDataStoreType(out settingValue)).Returns(true);

            var accountDataStore = new Mock<IAccountDataStore>();
            var backupAccountDataStore = new Mock<IAccountDataStore>();
            var service = new PaymentService(configuration.Object, accountDataStore.Object, backupAccountDataStore.Object);

            // Act
            MakePaymentResult result = service.MakePayment(new MakePaymentRequest());

            // Assert
            accountDataStore.Verify(ds => ds.GetAccount(It.IsAny<string>()), Times.Never);
            backupAccountDataStore.Verify(ds => ds.GetAccount(It.IsAny<string>()), Times.Once);
        }

        [TestCase(AllowedPaymentSchemes.Bacs, PaymentScheme.Bacs)]
        [TestCase(AllowedPaymentSchemes.Chaps, PaymentScheme.Chaps)]
        [TestCase(AllowedPaymentSchemes.FasterPayments, PaymentScheme.FasterPayments)]
        public void MakePayment_ReturnsSuccess(AllowedPaymentSchemes allowedScheme, PaymentScheme paymentScheme)
        {
            // Arrange
            var account = new Account
            {
                AllowedPaymentSchemes = allowedScheme,
                Status = AccountStatus.Live,
                Balance = 100
            };

            var configuration = new Mock<IConfigurationProvider>();
            var accountDataStore = new Mock<IAccountDataStore>();
            accountDataStore.Setup(ds => ds.GetAccount(It.IsAny<string>())).Returns(account);

            var backupAccountDataStore = new Mock<IAccountDataStore>();
            var service = new PaymentService(configuration.Object, accountDataStore.Object, backupAccountDataStore.Object);

            var request = new MakePaymentRequest
            {
                PaymentScheme = paymentScheme,
                Amount = 10
            };

            // Act
            MakePaymentResult result = service.MakePayment(request);

            // Assert
            Assert.IsTrue(result.Success);
            accountDataStore.Verify(ds => ds.UpdateAccount(account), Times.Once);
        }

        [TestCase(100, 10, 90)]
        [TestCase(1000.01, 10.50, 989.51)]
        [TestCase(999_999_999, 999_000_999, 999_000)]
        public void MakePayment_DecrementsBalance(decimal startAmount, decimal paymentAmount, decimal endAmount)
        {
            // Arrange
            var account = new Account
            {
                AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps,
                Status = AccountStatus.Live,
                Balance = startAmount
            };

            var configuration = new Mock<IConfigurationProvider>();
            var accountDataStore = new Mock<IAccountDataStore>();
            accountDataStore.Setup(ds => ds.GetAccount(It.IsAny<string>())).Returns(account);

            var backupAccountDataStore = new Mock<IAccountDataStore>();
            var service = new PaymentService(configuration.Object, accountDataStore.Object, backupAccountDataStore.Object);

            var request = new MakePaymentRequest
            {
                PaymentScheme = PaymentScheme.Chaps,
                Amount = paymentAmount
            };

            // Act
            MakePaymentResult result = service.MakePayment(request);

            // Assert
            accountDataStore.Verify(ds => ds.UpdateAccount(It.Is<Account>(a => a.Balance == endAmount)), Times.Once);
        }
    }
}
