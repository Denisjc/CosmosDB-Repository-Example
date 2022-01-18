using System;
using System.Net.Http;
using System.Threading.Tasks;
using Bogus;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using UI.FunctionApp.Tests.Helpers;
using Xunit;
using Database = Microsoft.Azure.Cosmos.Database;

namespace UI.FunctionApp.Tests
{
    public abstract class BaseTests : IAsyncLifetime
    {
        protected readonly IConfiguration _configuration;
        protected readonly ILogger _logger = LoggerHelper.CreateLogger(LoggerTypes.List);
        protected readonly Faker _faker;
        protected CosmosClient _cosmosClient;
        protected Database _database;
        protected Container _container;

        protected string _databaseId;
        private string containerId = "todos";

        //private static string _endpointUri;
        //private static string _primaryKey;

        protected  BaseTests()
        {
            _faker = new Faker();

            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var settingsFile = env == "Staging" ? "appsettings.Staging.json" : "appsettings.Development.json";
           
            _configuration = new ConfigurationBuilder()
                .AddJsonFile(settingsFile)
                .Build();

            var endpointUri = _configuration["AzureCosmosOptions:EndpointUri"];
            var primaryKey = _configuration["AzureCosmosOptions:PrimaryKey"];

            _databaseId = _configuration["AzureCosmosOptions:DatabaseId"];
            _cosmosClient = new CosmosClient(endpointUri, primaryKey, GetCosmosClientOptions());

        }

        private static CosmosClientOptions GetCosmosClientOptions()
        {
            var cosmosClientOptions = new CosmosClientOptions()
            {
                HttpClientFactory = () =>
                {
                    HttpMessageHandler httpMessageHandler = new HttpClientHandler()
                    {
                        ServerCertificateCustomValidationCallback =
                            HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                    };

                    return new HttpClient(httpMessageHandler);
                },
                ConnectionMode = ConnectionMode.Gateway
            };
            return cosmosClientOptions;
        }

        public Task DisposeAsync()
        {
            _cosmosClient = null;

            return Task.CompletedTask;
        }

        public async Task InitializeAsync()
        {
            try
            {
                _database = await _cosmosClient.CreateDatabaseIfNotExistsAsync(_databaseId);
                _container = await _database.CreateContainerIfNotExistsAsync(containerId, "/toDoId");

                //var cosmosDatabase =
                //    _cosmosClient.GetDatabase(
                //        _configuration["AzureCosmosOptions:DatabaseId"]);

                //await cosmosDatabase.CreateContainerIfNotExistsAsync(
                //    new ContainerProperties
                //    {
                //        Id = "todos",
                //        PartitionKeyPath = "/toDoId"
                //    });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
         
        }
    }
}
