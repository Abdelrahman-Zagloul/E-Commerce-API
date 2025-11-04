using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities;
using E_Commerce.Persistence.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Persistence.Repositories
{
    public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        private readonly StoreDbContext _context;

        public GenericRepository(StoreDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync() => await _context.Set<TEntity>().ToListAsync();

        public async Task<TEntity?> GetByIdAsync(TKey id) => await _context.Set<TEntity>().FindAsync(id);

        public async Task AddAsync(TEntity entity) => await _context.Set<TEntity>().AddAsync(entity);

        public void Update(TEntity entity) => _context.Set<TEntity>().Update(entity);

        public void Delete(TEntity entity) => _context.Set<TEntity>().Remove(entity);


        public async Task<IEnumerable<TEntity>> GetAllAsync(ISpecification<TEntity, TKey> specification)
            => await ApplaySpecification(specification).ToListAsync();

        public async Task<TEntity?> GetByIdAsync(ISpecification<TEntity, TKey> specification)
            => await ApplaySpecification(specification).FirstOrDefaultAsync();

        public async Task<int> CountAsync(ISpecification<TEntity, TKey> specification)
            => await ApplaySpecification(specification).CountAsync();




        private IQueryable<TEntity> ApplaySpecification(ISpecification<TEntity, TKey> specification)
            => SpecificationEvaluation.CreateQuery(_context.Set<TEntity>(), specification);

    }
}
