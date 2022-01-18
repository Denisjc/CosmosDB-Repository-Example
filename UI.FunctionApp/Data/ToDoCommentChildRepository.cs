using System.Collections.Generic;
using System.Threading.Tasks;
using Data.CosmosDb;
using Domain.Common.Entities;

namespace UI.FunctionApp.Data
{
    public class ToDoCommentChildRepository : ChildRepository<string, string, ToDoCommentEntity>, IToDoCommentChildRepository
    {
        public ToDoCommentChildRepository(
            RepositoryOptions entityDataStoreOptions) : base("todos", entityDataStoreOptions)
        {
        }

        public async Task<IEnumerable<ToDoCommentEntity>> ListByToDoIdAsync(
            string toDoId)
        {
            var query =
                "SELECT * FROM s WHERE s.object = 'Comment'";

            return await base.ListAsync(toDoId, query);
        }
    }
}
