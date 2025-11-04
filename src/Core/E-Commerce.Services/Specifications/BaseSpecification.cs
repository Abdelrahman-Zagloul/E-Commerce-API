using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities;
using System.Linq.Expressions;

namespace E_Commerce.Services.Specifications
{
    internal abstract class BaseSpecification<TEntity, TKey> : ISpecification<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        public int Take { get; private set; }
        public int Skip { get; private set; }
        public bool IsPaginationEnabled { get; private set; }
        public bool IsTrackingEnabled { get; private set; } = true;
        public Expression<Func<TEntity, bool>> Criteria { get; private set; }
        public Expression<Func<TEntity, object>> OrderBy { get; private set; }
        public Expression<Func<TEntity, object>> OrderByDescending { get; private set; }
        public ICollection<Expression<Func<TEntity, object>>> IncludeExpressions { get; } = [];

        protected BaseSpecification(Expression<Func<TEntity, bool>> criteria)
        {
            Criteria = criteria;
        }

        protected void AddInclude(Expression<Func<TEntity, object>> includeExpression)
            => IncludeExpressions.Add(includeExpression);
        protected void AddOrderBy(Expression<Func<TEntity, object>> orderByExpression)
            => OrderBy = orderByExpression;

        protected void AddOrderByDescending(Expression<Func<TEntity, object>> orderByDescExpression)
            => OrderByDescending = orderByDescExpression;

        protected void DisableTracking()
            => IsTrackingEnabled = false;

        protected void ApplayPagination(int pageIndex, int pageSize)
        {
            IsPaginationEnabled = true;
            Take = pageSize;
            Skip = (pageIndex - 1) * pageSize;
        }
    }
}
