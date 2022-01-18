namespace Data.CosmosDb.Tests.Helpers
{
    public class FakeEntityDataStore : Repository<string, FakeEntity>
    {
        public FakeEntityDataStore(
               RepositoryOptions entityDataStoreOptions) : base("fakes", entityDataStoreOptions)
        {
        }
    }
}
