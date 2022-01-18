using Data.CosmosDb;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UI.FunctionApp.Data;

namespace UI.FunctionApp.Extensions
{
    public static class ServicesExtensions
    {
        public static void SetupCosmosDb(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config["AzureCosmosOptions:ConnectionString"];
            var databaseId = config["AzureCosmosOptions:DatabaseId"];

            var entityDataStoreOptions = new RepositoryOptions();

            var cosmosClient = new CosmosClientBuilder(connectionString)
                .WithConnectionModeDirect()
                .Build();

            entityDataStoreOptions.CosmosClient = cosmosClient;
            entityDataStoreOptions.DatabaseId = databaseId;

            services.AddSingleton(entityDataStoreOptions);

            services.AddTransient<IToDoRepository, ToDoRepository>();
            services.AddTransient<IToDoCommentChildRepository, ToDoCommentChildRepository>();
        }
    }
}
