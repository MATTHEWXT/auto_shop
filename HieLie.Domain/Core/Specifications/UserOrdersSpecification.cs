using HieLie.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HieLie.Domain.Core.Specifications
{
    public class UserOrdersSpecification
    {
        public static BaseSpecification<Order> GetUserOrders(Guid custId)
        {
            return new BaseSpecification<Order>(o => o.CustomerId == custId);
        }

        public static BaseSpecification<OrderItem> GetOrderItems(Guid orderId)
        {
            var specification = new BaseSpecification<OrderItem>(oi => oi.OrderId == orderId);
            specification.AddInclude(oi => oi.Product);

            return specification;
        }
    }
}
