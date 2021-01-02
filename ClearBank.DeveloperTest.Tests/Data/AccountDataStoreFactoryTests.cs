using ClearBank.DeveloperTest.Configuration;
using ClearBank.DeveloperTest.Data;
using Moq;
using NUnit.Framework;

namespace ClearBank.DeveloperTest.Tests.Data
{
    [TestFixture]
    public class AccountDataStoreFactoryTests
    {
        [TestCase("Live")]
        [TestCase("")]
        [TestCase(null)]
        public void DefaultsToLiveDataStore(string settingValue)
        {
            // Arrange
            var configuration = new Mock<IConfigurationProvider>();
            configuration.Setup(c => c.TryGetDataStoreType(out settingValue)).Returns(true);
            var factory = new AccountDataStoreFactory(configuration.Object);

            // Act
            IAccountDataStore datastore = factory.BuildAccountDataStore();

            // Assert
            Assert.AreEqual(typeof(AccountDataStore), datastore.GetType());
        }

        [Test]
        public void UsesLiveDataStoreWhenProviderReturnsFalse()
        {
            // Arrange
            string settingValue;
            var configuration = new Mock<IConfigurationProvider>();
            configuration.Setup(c => c.TryGetDataStoreType(out settingValue)).Returns(false);
            var factory = new AccountDataStoreFactory(configuration.Object);

            // Act
            IAccountDataStore datastore = factory.BuildAccountDataStore();

            // Assert
            Assert.AreEqual(typeof(AccountDataStore), datastore.GetType());
        }

        [Test]
        public void UsesBackupDataStoreWhenConfigured()
        {
            // Arrange
            string settingValue = "Backup";
            var configuration = new Mock<IConfigurationProvider>();
            configuration.Setup(c => c.TryGetDataStoreType(out settingValue)).Returns(true);
            var factory = new AccountDataStoreFactory(configuration.Object);

            // Act
            IAccountDataStore datastore = factory.BuildAccountDataStore();

            // Assert
            Assert.AreEqual(typeof(BackupAccountDataStore), datastore.GetType());
        }
    }
}
