using Microsoft.Azure.Cosmos;

namespace Data.CosmosDb
{
    public class RepositoryOptions
    {
        public string DatabaseId { get; set; }
        public CosmosClient CosmosClient { get; set; }

        public RepositoryOptions()
        {
        }

        public RepositoryOptions(
            string databaseId,
            CosmosClient cosmosClient)
        {
            this.DatabaseId = databaseId;
            this.CosmosClient = cosmosClient;
        }
    }
}
