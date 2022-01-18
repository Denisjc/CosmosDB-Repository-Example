using System.Collections.Generic;
using System.Threading.Tasks;
using Data.CosmosDb;
using Domain.Common.Entities;

namespace UI.Function.Main.Data
{
    public interface IToDoCommentChildRepository : IChildIRepository<string, string, ToDoCommentEntity>
    {
        Task<IEnumerable<ToDoCommentEntity>> ListByToDoIdAsync(
            string toDoId);
    }
}
