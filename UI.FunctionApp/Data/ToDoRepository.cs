using System.Collections.Generic;
using System.Threading.Tasks;
using Data.CosmosDb;
using Domain.Common.Entities;

namespace UI.FunctionApp.Data
{
    public class ToDoRepository : Repository<string, ToDoEntity>, IToDoRepository
    {
        public ToDoRepository(
            RepositoryOptions entityDataStoreOptions) : base("todos", entityDataStoreOptions)
        {
        }

        public async Task<IEnumerable<ToDoEntity>> ListAsync()
        {
            var query =
                "SELECT * FROM s WHERE s.object = 'ToDo'";

            return await base.ListAsync(query);
        }
    }
}
