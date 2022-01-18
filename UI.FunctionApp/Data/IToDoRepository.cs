using System.Collections.Generic;
using System.Threading.Tasks;
using Data.CosmosDb;
using Domain.Common.Entities;

namespace UI.FunctionApp.Data
{
    public interface IToDoRepository : IRepository<string, ToDoEntity>
    {
        Task<IEnumerable<ToDoEntity>> ListAsync();
    }
}
