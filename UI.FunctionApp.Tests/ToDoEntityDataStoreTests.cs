using System;
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
    public class ToDoEntityDataStoreTests : BaseTests
    {
        [Fact]
        public async Task When_AddAsync()
        {
            // Arrange
            var entityDataStoreOptions =
                new RepositoryOptions
                {
                    CosmosClient = _cosmosClient,
                    DatabaseId = _databaseId
                };

            var repository = new ToDoRepository(entityDataStoreOptions);

            var toDoEntity = _faker.GenerateToDoEntity();

            // Action
            await repository.AddAsync(toDoEntity);

            // Assert
            var itemResponse =
                await _container.ReadItemAsync<ToDoEntity>(
                    toDoEntity.Id,
                    new PartitionKey(toDoEntity.Id));

            itemResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var toDoEntityFetched =
                itemResponse.Resource;

            toDoEntityFetched.Should().NotBeNull();
            toDoEntityFetched.Id.Should().Be(toDoEntity.Id);
            toDoEntityFetched.ToDoId.Should().Be(toDoEntity.ToDoId);
            toDoEntityFetched.ToDoId.Should().Be(toDoEntityFetched.Id);
            toDoEntityFetched.Status.Should().Be(toDoEntity.Status);
            toDoEntityFetched.Description.Should().Be(toDoEntity.Description);
        }

        [Fact]
        public async Task When_GetByIdAsync()
        {
            // Arrange
            var toDoEntity =
                _faker.GenerateToDoEntity();

            await _container.CreateItemAsync(toDoEntity, new PartitionKey(toDoEntity.ToDoId));

            var entityDataStoreOptions =
                new RepositoryOptions
                {
                    CosmosClient = _cosmosClient,
                    DatabaseId = _databaseId
                };

            var repository = new ToDoRepository(entityDataStoreOptions);

            // Action

            var toDoEntityFetched = await repository.GetByIdAsync(toDoEntity.Id);

            // Assert

            toDoEntityFetched.Should().NotBeNull();
            toDoEntityFetched.Id.Should().Be(toDoEntity.Id);
            toDoEntityFetched.ToDoId.Should().Be(toDoEntity.ToDoId);
            toDoEntityFetched.ToDoId.Should().Be(toDoEntityFetched.Id);
            toDoEntityFetched.Status.Should().Be(toDoEntity.Status);
            toDoEntityFetched.Description.Should().Be(toDoEntity.Description);
        }

        [Fact]
        public async Task When_GetByIdAsync_And_EntityDoesNotExist_Then_ReturnNull()
        {
            // Arrange

            var entityDataStoreOptions =
                new RepositoryOptions
                {
                    CosmosClient = _cosmosClient,
                    DatabaseId = _databaseId
                };

            var repository = new ToDoRepository(entityDataStoreOptions);

            // Action
            var toDoEntity = await repository.GetByIdAsync("IMABADID");

            // Assert
            toDoEntity.Should().BeNull();
        }

        [Fact]
        public async Task When_DeleteByIdAsync()
        {
            // Arrange
            var toDoEntity = _faker.GenerateToDoEntity();
            
            await _container.CreateItemAsync(toDoEntity, new PartitionKey(toDoEntity.ToDoId));

            var entityDataStoreOptions =
                new RepositoryOptions
                {
                    CosmosClient = _cosmosClient,
                    DatabaseId = _databaseId
                };

            var repository = new ToDoRepository(entityDataStoreOptions);

            // Action
            await repository.DeleteByIdAsync(toDoEntity.Id);

            // Assert
            Func<Task> action = async () =>
                await _container.ReadItemAsync<ToDoEntity>(
                    toDoEntity.Id,
                    new PartitionKey(toDoEntity.Id));

            //action.Should().Throw<CosmosException>();
        }

        [Fact]
        public async Task When_UpdateAsync()
        {
            // Arrange

            var toDoEntity = _faker.GenerateToDoEntity();
            
            await _container.CreateItemAsync(toDoEntity, new PartitionKey(toDoEntity.ToDoId));

            var entityDataStoreOptions =
                new RepositoryOptions
                {
                    CosmosClient = _cosmosClient,
                    DatabaseId = _databaseId
                };

            var repository = new ToDoRepository(entityDataStoreOptions);

            toDoEntity.Description = _faker.Lorem.Paragraph(1);

            // Action

            await repository.UpdateAsync(toDoEntity);

            // Assert
            var itemResponse = await _container.ReadItemAsync<ToDoEntity>(toDoEntity.Id, new PartitionKey(toDoEntity.Id));

            itemResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var toDoEntityUpdated = itemResponse.Resource;

            toDoEntityUpdated.Should().NotBeNull();
            toDoEntityUpdated.Description.Should().Be(toDoEntity.Description);
        }

        [Fact]
        public async Task When_ListAsync()
        {
            // Arrange
            for (var i = 0; i < 3; i++)
            {
                var toDoEntity = _faker.GenerateToDoEntity();

                await _container.CreateItemAsync(
                    toDoEntity,
                    new PartitionKey(toDoEntity.ToDoId));
            }

            var entityDataStoreOptions =
                new RepositoryOptions
                {
                    CosmosClient = _cosmosClient,
                    DatabaseId = _databaseId
                };

            var repository = new ToDoRepository(entityDataStoreOptions);

            // Action
            var toDoEntityFetchedList = await repository.ListAsync();

            // Assert
            toDoEntityFetchedList.Should().NotBeNull();
            toDoEntityFetchedList.Count().Should().BeGreaterOrEqualTo(3);
        }
    }
}