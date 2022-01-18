using System.Threading.Tasks;
using Domain.Core.Interfaces;

namespace Data.CosmosDb
{
    public interface IRepository<TKey, TEntity> where TEntity : IEntity<TKey>
    {
        Task<TEntity> AddAsync(TEntity entity);
        Task DeleteByIdAsync(TKey id);

        Task<TEntity> GetByIdAsync(TKey id);

        Task UpdateAsync(TEntity entity);
    }
}