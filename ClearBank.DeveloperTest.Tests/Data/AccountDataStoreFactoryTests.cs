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

            var liveStore = new Mock<IAccountDataStore>();
            var backupStore = new Mock<IAccountDataStore>();

            var factory = new AccountDataStoreFactory(configuration.Object, liveStore.Object, backupStore.Object);

            // Act
            IAccountDataStore datastore = factory.BuildAccountDataStore();

            // Assert
            Assert.AreEqual(liveStore.Object, datastore);
        }

        [Test]
        public void UsesLiveDataStoreWhenProviderReturnsFalse()
        {
            // Arrange
            string settingValue;

            var configuration = new Mock<IConfigurationProvider>();
            configuration.Setup(c => c.TryGetDataStoreType(out settingValue)).Returns(false);

            var liveStore = new Mock<IAccountDataStore>();
            var backupStore = new Mock<IAccountDataStore>();

            var factory = new AccountDataStoreFactory(configuration.Object, liveStore.Object, backupStore.Object);

            // Act
            IAccountDataStore datastore = factory.BuildAccountDataStore();

            // Assert
            Assert.AreEqual(liveStore.Object, datastore);
        }

        [Test]
        public void UsesBackupDataStoreWhenConfigured()
        {
            // Arrange
            string settingValue = "Backup";

            var configuration = new Mock<IConfigurationProvider>();
            configuration.Setup(c => c.TryGetDataStoreType(out settingValue)).Returns(true);
            var liveStore = new Mock<IAccountDataStore>();
            var backupStore = new Mock<IAccountDataStore>();

            var factory = new AccountDataStoreFactory(configuration.Object, liveStore.Object, backupStore.Object);

            // Act
            IAccountDataStore datastore = factory.BuildAccountDataStore();

            // Assert
            Assert.AreEqual(backupStore.Object, datastore);
        }
    }
}
