using HieLie.Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HieLie.Domain.Core.Repositories
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();
        Task RollBackAsync();
        IBaseRepositoryAsync<T> Repository<T>() where T : BaseEnity;
    }
}
