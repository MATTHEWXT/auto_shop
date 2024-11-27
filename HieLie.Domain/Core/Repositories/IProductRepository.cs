using HieLie.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HieLie.Domain.Core.Repositories
{
    public interface IProductRepository
    {
        Task<IList<Product>> GetAFewProducts();
    }
}
