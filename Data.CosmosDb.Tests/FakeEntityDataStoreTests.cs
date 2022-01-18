using System;
using Data.CosmosDb.Tests.Helpers;
using FluentAssertions;
using Xunit;

namespace Data.CosmosDb.Tests
{
    public class FakeEntityDataStoreTests : BaseTests
    {
        [Fact]
        public void When_Create_And_EntityDataStoreOptionsIsNull_Then_ThrowsArgumentNullException()
        {
            // Action

            Action action = () => new FakeEntityDataStore(
                null);

            // Assert

            action.Should().Throw<ArgumentNullException>();
        }


        [Fact]
        public void When_Create_And_EntityDataStoreOptionsCloudClientIsNull_Then_ThrowsArgumentNullException()
        {
            // Arrange

            var entityDataStoreOptions =
                new RepositoryOptions();

            // Action

            Action action = () => new FakeEntityDataStore(
                entityDataStoreOptions);

            // Assert

            action.Should().Throw<ArgumentNullException>();
        }
    }
}
