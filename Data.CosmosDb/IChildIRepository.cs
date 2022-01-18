using System.Threading.Tasks;
using Domain.Core.Interfaces;

namespace Data.CosmosDb
{
    public interface IChildIRepository<TParentKey, TKey, TEntity> where TEntity : IChildEntity<TKey>
    {
        Task AddAsync(
            TParentKey parentId,
            TEntity entity);

        Task DeleteByIdAsync(
            TParentKey parentId,
            TKey id);

        Task<TEntity> GetByIdAsync(
            TParentKey parentId,
            TKey id);

        Task UpdateAsync(
            TParentKey parentId,
            TEntity entity);
    }
}