using HieLie.Domain.Core.Models;
using HieLie.Domain.Core.Repositories;
using HieLie.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HieLie.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ShopDbContext _dbContext;
        private readonly IDictionary<Type, dynamic> _repositories;

        public UnitOfWork(ShopDbContext dbContext)
        {
            _dbContext = dbContext;
            _repositories = new Dictionary<Type, dynamic>();
        }

        public IBaseRepositoryAsync<T> Repository<T>() where T : BaseEnity
        {
            var entityType = typeof(T);

            if (_repositories.ContainsKey(entityType)) { 
                return _repositories[entityType];
            }

            var repositoryType = typeof(BaseRepositoryAsync<>);

            var repository = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), _dbContext);

            if (repository == null)
            {
                throw new NullReferenceException("Repository should not be null");
            }

            _repositories.Add(entityType, repository);

            return (IBaseRepositoryAsync<T>)repository;
        }

        public Task RollBackAsync()
        {
            throw new NotImplementedException();
        }

        public Task<int> SaveChangesAsync()
        {
            throw new NotImplementedException();
        }
    }
}
