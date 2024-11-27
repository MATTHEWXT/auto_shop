using HieLie.Domain.Core.Models;
using HieLie.Domain.Core.Repositories;
using HieLie.Domain.Core.Specifications;
using HieLie.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HieLie.Infrastructure.Repositories
{

    public class BaseRepositoryAsync<T> : IBaseRepositoryAsync<T> where T : BaseEnity
    {
        private readonly ShopDbContext _dbContext;

        public BaseRepositoryAsync(ShopDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<T> AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            _dbContext.SaveChanges();
            return entity;
        }

        public async Task AddListAsync(IList<T> entities)
        {
            await _dbContext.Set<T>().AddRangeAsync(entities);
            _dbContext.SaveChanges();
        }

        public async Task DeleteEntitiesAsync(IList<T> entities)
        {
            _dbContext.Set<T>().RemoveRange(entities);
            await _dbContext.SaveChangesAsync();
        }

        public void Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);

            _dbContext.SaveChanges();
        }

        public async Task<T> FirstOrDefaultAsync(IBaseSpecification<T> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync() ?? throw new InvalidOperationException("Entity not found");
        }

        public async Task<IList<T>> GetAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }
        public async Task<IList<T>> ListAsync(IBaseSpecification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }
     
        public async Task<T> GetByIdAsync(Guid id)
        {
            return await _dbContext.Set<T>().FindAsync(id) ?? throw new InvalidOperationException("Entity not found");
        }

        public void Update(T entity)
        {
            _dbContext.Set<T>().Update(entity);

            _dbContext.SaveChanges();
        }

        private IQueryable<T> ApplySpecification(IBaseSpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_dbContext.Set<T>(), spec);
        }
    }
}
