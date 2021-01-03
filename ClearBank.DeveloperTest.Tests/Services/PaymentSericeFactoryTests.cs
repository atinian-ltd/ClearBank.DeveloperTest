using ClearBank.DeveloperTest.Services;
using NUnit.Framework;

namespace ClearBank.DeveloperTest.Tests.Services
{
    [TestFixture]
    public class PaymentSericeFactoryTests
    {
        [Test]
        public void BuildsService()
        {
            // Arrange
            var factory = new PaymentServiceFactory();

            // Act
            var service = factory.BuildPaymentService();

            // Assert
            Assert.IsNotNull(service);
        }
    }
}
