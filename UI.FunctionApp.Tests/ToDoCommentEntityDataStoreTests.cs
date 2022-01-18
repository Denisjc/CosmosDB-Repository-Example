using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Data.CosmosDb;
using Domain.Common.Entities;
using FluentAssertions;
using Microsoft.Azure.Cosmos;
using UI.FunctionApp.Data;
using UI.FunctionApp.Tests.Helpers;
using Xunit;

namespace UI.FunctionApp.Tests
{
    public class ToDoCommentEntityDataStoreTests : BaseTests
    {
        [Fact]
        public async Task When_AddAsync()
        {
            // Arrange

            var cosmosDatabase =
                _cosmosClient.GetDatabase(
                    _configuration["AzureCosmosOptions:DatabaseId"]);

            var cosmosContainer =
                cosmosDatabase.GetContainer("todos");

            var toDoEntity =
                _faker.GenerateToDoEntity();

            await cosmosContainer.CreateItemAsync(
                toDoEntity,
                new PartitionKey(toDoEntity.Id));

            var entityDataStoreOptions =
                new RepositoryOptions
                {
                    CosmosClient = _cosmosClient,
                    DatabaseId = _configuration["AzureCosmosOptions:DatabaseId"]
                };

            var repository =
                new ToDoCommentChildRepository(
                    entityDataStoreOptions);

            var toDoCommentEntity =
                _faker.GenerateToDoCommentEntity(
                    toDoEntity.Id);

            // Action

            await repository.AddAsync(
                toDoEntity.Id,
                toDoCommentEntity);

            // Assert

            var itemResponse =
                await cosmosContainer.ReadItemAsync<ToDoCommentEntity>(
                    toDoCommentEntity.Id,
                    new PartitionKey(toDoEntity.Id));

            itemResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var toDoCommentEntityFetched =
                itemResponse.Resource;

            toDoCommentEntityFetched.Should().NotBeNull();
            toDoCommentEntityFetched.Id.Should().Be(toDoCommentEntity.Id);
            toDoCommentEntityFetched.ToDoId.Should().Be(toDoCommentEntity.ToDoId);
            toDoCommentEntityFetched.Body.Should().Be(toDoCommentEntity.Body);
        }

        [Fact]
        public async Task When_ListAsync()
        {
            // Arrange

            var cosmosDatabase =
                 _cosmosClient.GetDatabase(
                     _configuration["AzureCosmosOptions:DatabaseId"]);

            var cosmosContainer =
                cosmosDatabase.GetContainer("todos");

            var toDoEntity =
                _faker.GenerateToDoEntity();

            await cosmosContainer.CreateItemAsync(
                toDoEntity,
                new PartitionKey(toDoEntity.ToDoId));

            for (var i = 0; i < 3; i++)
            {
                var toDoCommentEntity =
                    _faker.GenerateToDoCommentEntity(
                        toDoEntity.Id);

                await cosmosContainer.CreateItemAsync(
                    toDoCommentEntity,
                    new PartitionKey(toDoEntity.Id));
            }

            var entityDataStoreOptions =
                new RepositoryOptions
                {
                    CosmosClient = _cosmosClient,
                    DatabaseId = _configuration["AzureCosmosOptions:DatabaseId"]
                };

            var repository =
                new ToDoCommentChildRepository(
                    entityDataStoreOptions);

            // Action

            var toDoCommentEntityList =
                await repository.ListByToDoIdAsync(
                    toDoEntity.Id);

            // Assert

            toDoCommentEntityList.Should().NotBeNull();
            toDoCommentEntityList.Count().Should().Be(3);
        }
    }
}
