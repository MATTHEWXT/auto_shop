using HieLie.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace HieLie.Domain.Core.Specifications
{
    public class CategorySpecification
    {
        public static BaseSpecification<Category> GetCategoryByName(string name)
        {
            return new BaseSpecification<Category>(c => c.Name == name);
        }
    }
}
