using HieLie.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HieLie.Domain.Core.Specifications
{
    public class BasketSpecification
    {
        public static BaseSpecification<Basket> IsBasketByUserId(Guid userId)
        {
            return new BaseSpecification<Basket>(b => b.UserId == userId);
        }

        public static BaseSpecification<Basket> GetBasketByUserId(Guid userId)
        {
            return new BaseSpecification<Basket>(b => b.UserId == userId);
        }

        public static BaseSpecification<Customer> GetCustomerByUserId(Guid userId)
        {
            return new BaseSpecification<Customer>(c => c.UserId == userId);
        }

        public static BaseSpecification<Customer> GetCustomerById(Guid custId)
        {
            return new BaseSpecification<Customer>(c => c.Id == custId);
        }

    }
}

