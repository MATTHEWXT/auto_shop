using HieLie.Application.Models.Request;
using HieLie.Domain.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HieLie.Application.Interfaces
{
    public interface ICacheService
    {
        Task SetAsync(string key, string value, TimeSpan? expiry = null);
        Task<string?> GetAsync(string key);
        Task RemoveAsync(string key);
        Task AddCategory(Guid categoryId, string categoryName);
        Task<string> GetCategory(Guid categoryId);
        Task SetProduct(Product product);
        Task SetListProducts(List<Product> product);
        Task<List<Product>> GetProducts(Guid categoryId);
    }
}
