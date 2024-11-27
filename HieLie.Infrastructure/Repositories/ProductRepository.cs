using HieLie.Domain.Core.Repositories;
using HieLie.Domain.Entities;
using HieLie.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HieLie.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ShopDbContext _dbContext;

        public ProductRepository(ShopDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IList<Product>> GetAFewProducts()
        {
            var aFewProductsGroup = await _dbContext.Products
                .GroupBy(p => p.CategoryId)
                .ToListAsync();
               
            var aFewProducts = aFewProductsGroup
                .SelectMany(g => g.OrderBy(p => Guid.NewGuid()).Take(2))
                .Where(p => p != null)
                .ToList();

            return aFewProducts;
        }
    }
}
