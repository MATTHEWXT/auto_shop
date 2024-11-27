using HieLie.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HieLie.Domain.Core.Specifications
{
    public class ProductSpecification
    {
        public static BaseSpecification<Category> GetProductsByCategory(string categoryName)
        {
            var specification = new BaseSpecification<Category>(c => c.Name == categoryName);
            specification.AddInclude(c => c.Products!);

            return specification;
        }

        public static BaseSpecification<Product> GetProductsBySearchTerm(string searchTerm)
        {
            return new BaseSpecification<Product>(p => p.Name.Contains(searchTerm));
        }

        public static BaseSpecification<Product> GetProductByName(string productName)
        {
            return new BaseSpecification<Product>(p => p.Name == productName);
        }
    }
}
