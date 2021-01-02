using ClearBank.DeveloperTest.Configuration;
using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Types;
using ClearBank.DeveloperTest.Validation;
using Moq;
using NUnit.Framework;

namespace ClearBank.DeveloperTest.Tests.Services
{
    [TestFixture]
    public class PaymentsServiceTests
    {
        [Test]
        public void MakePayment_ReturnsNotNull()
        {
            // Arrange
            var dataStore = new Mock<IAccountDataStore>();
            dataStore.Setup(ds => ds.GetAccount(It.IsAny<string>())).Returns(new Account());

            var accountDataStoreFactory = new Mock<IAccountDataStoreFactory>();
            accountDataStoreFactory.Setup(f => f.BuildAccountDataStore()).Returns(dataStore.Object);

            var validator = new Mock<IValidator>();
            validator.Setup(v => v.IsValid(It.IsAny<Account>(), It.IsAny<decimal>())).Returns(true);

            var validatorFactory = new Mock<IValidatorFactory>();
            validatorFactory.Setup(vf => vf.BuildValidator(It.IsAny<PaymentScheme>())).Returns(validator.Object);

            var service = new PaymentService(accountDataStoreFactory.Object, validatorFactory.Object);

            // Act
            MakePaymentResult result = service.MakePayment(new MakePaymentRequest());

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void MakePayment_ReturnsFailureIfNullRequest()
        {
            // Arrange
            var dataStore = new Mock<IAccountDataStore>();
            dataStore.Setup(ds => ds.GetAccount(It.IsAny<string>())).Returns((Account)null);

            var accountDataStoreFactory = new Mock<IAccountDataStoreFactory>();
            accountDataStoreFactory.Setup(f => f.BuildAccountDataStore()).Returns(dataStore.Object);

            var validator = new Mock<IValidator>();
            validator.Setup(v => v.IsValid(It.IsAny<Account>(), It.IsAny<decimal>())).Returns(true);

            var validatorFactory = new Mock<IValidatorFactory>();
            validatorFactory.Setup(vf => vf.BuildValidator(It.IsAny<PaymentScheme>())).Returns(validator.Object);

            var service = new PaymentService(accountDataStoreFactory.Object, validatorFactory.Object);

            // Act
            MakePaymentResult result = service.MakePayment(null);

            // Assert
            Assert.IsFalse(result.Success);
            dataStore.Verify(ds => ds.UpdateAccount(It.IsAny<Account>()), Times.Never);
        }

        [Test]
        public void MakePayment_ReturnsFailureIfAccountNotFound()
        {
            // Arrange
            var dataStore = new Mock<IAccountDataStore>();
            dataStore.Setup(ds => ds.GetAccount(It.IsAny<string>())).Returns((Account)null);

            var accountDataStoreFactory = new Mock<IAccountDataStoreFactory>();
            accountDataStoreFactory.Setup(f => f.BuildAccountDataStore()).Returns(dataStore.Object);

            var validator = new Mock<IValidator>();
            validator.Setup(v => v.IsValid(It.IsAny<Account>(), It.IsAny<decimal>())).Returns(true);

            var validatorFactory = new Mock<IValidatorFactory>();
            validatorFactory.Setup(vf => vf.BuildValidator(It.IsAny<PaymentScheme>())).Returns(validator.Object);

            var service = new PaymentService(accountDataStoreFactory.Object, validatorFactory.Object);

            // Act
            MakePaymentResult result = service.MakePayment(new MakePaymentRequest());

            // Assert
            Assert.IsFalse(result.Success);
            dataStore.Verify(ds => ds.UpdateAccount(It.IsAny<Account>()), Times.Never);
        }

        [Test]
        public void MakePayment_ReturnsFailureIfValidationFails()
        {
            // Arrange
            var dataStore = new Mock<IAccountDataStore>();

            var accountDataStoreFactory = new Mock<IAccountDataStoreFactory>();
            accountDataStoreFactory.Setup(f => f.BuildAccountDataStore()).Returns(dataStore.Object);

            var validator = new Mock<IValidator>();
            validator.Setup(v => v.IsValid(It.IsAny<Account>(), It.IsAny<decimal>())).Returns(false);

            var validatorFactory = new Mock<IValidatorFactory>();
            validatorFactory.Setup(vf => vf.BuildValidator(It.IsAny<PaymentScheme>())).Returns(validator.Object);

            var service = new PaymentService(accountDataStoreFactory.Object, validatorFactory.Object);

            // Act
            MakePaymentResult result = service.MakePayment(new MakePaymentRequest());

            // Assert
            Assert.IsFalse(result.Success);
            dataStore.Verify(ds => ds.UpdateAccount(It.IsAny<Account>()), Times.Never);
        }

        [Test]
        public void MakePayment_CallsFactoryForDataStore()
        {
            // Arrange
            var dataStore = new Mock<IAccountDataStore>();

            var accountDataStoreFactory = new Mock<IAccountDataStoreFactory>();
            accountDataStoreFactory.Setup(f => f.BuildAccountDataStore()).Returns(dataStore.Object);

            var validator = new Mock<IValidator>();
            var validatorFactory = new Mock<IValidatorFactory>();
            validatorFactory.Setup(vf => vf.BuildValidator(It.IsAny<PaymentScheme>())).Returns(validator.Object);

            var service = new PaymentService(accountDataStoreFactory.Object, validatorFactory.Object);

            // Act
            MakePaymentResult result = service.MakePayment(new MakePaymentRequest());

            // Assert
            accountDataStoreFactory.Verify(f => f.BuildAccountDataStore(), Times.Once);
        }

        [Test]
        public void MakePayment_ReturnsSuccess()
        {
            // Arrange
            var dataStore = new Mock<IAccountDataStore>();
            dataStore.Setup(ds => ds.GetAccount(It.IsAny<string>())).Returns(new Account());

            var accountDataStoreFactory = new Mock<IAccountDataStoreFactory>();
            accountDataStoreFactory.Setup(f => f.BuildAccountDataStore()).Returns(dataStore.Object);

            var validator = new Mock<IValidator>();
            validator.Setup(v => v.IsValid(It.IsAny<Account>(), It.IsAny<decimal>())).Returns(true);

            var validatorFactory = new Mock<IValidatorFactory>();
            validatorFactory.Setup(vf => vf.BuildValidator(It.IsAny<PaymentScheme>())).Returns(validator.Object);

            var service = new PaymentService(accountDataStoreFactory.Object, validatorFactory.Object);

            // Act
            MakePaymentResult result = service.MakePayment(new MakePaymentRequest());

            // Assert
            Assert.IsTrue(result.Success);
            dataStore.Verify(ds => ds.UpdateAccount(It.IsAny<Account>()), Times.Once);
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

            var dataStore = new Mock<IAccountDataStore>();
            dataStore.Setup(ds => ds.GetAccount(It.IsAny<string>())).Returns(account);

            var accountDataStoreFactory = new Mock<IAccountDataStoreFactory>();
            accountDataStoreFactory.Setup(f => f.BuildAccountDataStore()).Returns(dataStore.Object);

            var validator = new Mock<IValidator>();
            validator.Setup(v => v.IsValid(It.IsAny<Account>(), It.IsAny<decimal>())).Returns(true);

            var validatorFactory = new Mock<IValidatorFactory>();
            validatorFactory.Setup(vf => vf.BuildValidator(It.IsAny<PaymentScheme>())).Returns(validator.Object);

            var service = new PaymentService(accountDataStoreFactory.Object, validatorFactory.Object);

            var request = new MakePaymentRequest
            {
                PaymentScheme = PaymentScheme.Chaps,
                Amount = paymentAmount
            };

            // Act
            MakePaymentResult result = service.MakePayment(request);

            // Assert
            dataStore.Verify(ds => ds.UpdateAccount(It.Is<Account>(a => a.Balance == endAmount)), Times.Once);
        }
    }
}
