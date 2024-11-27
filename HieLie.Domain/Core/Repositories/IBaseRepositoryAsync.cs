
using HieLie.Domain.Core.Models;
using HieLie.Domain.Core.Specifications;

namespace HieLie.Domain.Core.Repositories
{
    public interface IBaseRepositoryAsync<T> where T : BaseEnity
    {
        Task<T> GetByIdAsync(Guid id);
        Task<IList<T>> GetAllAsync();
        Task<IList<T>> ListAsync(IBaseSpecification<T> spec);
        Task<T> AddAsync(T entity);
        Task AddListAsync(IList<T> entities);
        Task<T> FirstOrDefaultAsync(IBaseSpecification<T> spec);
        void Update(T entity);
        void Delete(T entity);
        Task DeleteEntitiesAsync(IList<T> entities);
    }
}
