using E_Commerce.Domain.Entities;
using System.Linq.Expressions;

namespace E_Commerce.Domain.Contracts
{
    public interface ISpecification<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        Expression<Func<TEntity, bool>> Criteria { get; }
        Expression<Func<TEntity, object>> OrderBy { get; }
        Expression<Func<TEntity, object>> OrderByDescending { get; }
        ICollection<Expression<Func<TEntity, object>>>? IncludeExpressions { get; }

        int Take { get; }
        int Skip { get; }
        bool IsPaginationEnabled { get; }
        bool IsTrackingEnabled { get; }
    }
}
