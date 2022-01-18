using Data.CosmosDb;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace UI.Function.Main.Extensions
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
        }
    }
}
