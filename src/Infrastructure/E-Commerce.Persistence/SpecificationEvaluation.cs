using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Persistence
{
    public static class SpecificationEvaluation
    {
        public static IQueryable<TEntity> CreateQuery<TEntity, TKey>(IQueryable<TEntity> inputQuery, ISpecification<TEntity, TKey> specification) where TEntity : BaseEntity<TKey>
        {
            IQueryable<TEntity> query = inputQuery;

            if (!specification.IsTrackingEnabled)
                query = query.AsNoTracking();

            if (specification.Criteria != null)
                query = query.Where(specification.Criteria);

            if (specification.OrderBy != null)
                query = query.OrderBy(specification.OrderBy);

            if (specification.OrderByDescending != null)
                query = query.OrderByDescending(specification.OrderByDescending);

            if (specification.IsPaginationEnabled)
                query = query.Skip(specification.Skip).Take(specification.Take);

            if (specification.IncludeExpressions != null && specification.IncludeExpressions.Any())
                query = specification.IncludeExpressions.Aggregate(query, (currentQuery, include) => currentQuery.Include(include));


            return query;
        }
    }
}
