using E_Commerce.Domain.Entities;

namespace E_Commerce.Domain.Contracts
{
    public interface IUnitOfWork
    {
        IGenericRepository<TEntity, TKey> Repository<TEntity, TKey>() where TEntity : BaseEntity<TKey>;
        Task<int> SaveChangesAsync();
    }
}
