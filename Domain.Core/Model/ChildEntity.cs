using Domain.Core.Interfaces;

namespace Domain.Core.Model
{
    public abstract class ChildEntity<TKey> : Entity<TKey>, IChildEntity<TKey>
    {
    }
}
