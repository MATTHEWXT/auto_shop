using HieLie.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HieLie.Domain.Core.Specifications
{
    public class BasketItemSpecification
    {
        public static BaseSpecification<BasketItem> IsBasketItem(Guid busketId, Guid productId)
        {
            return new BaseSpecification<BasketItem>(bi => bi.BasketId == busketId && bi.ProductId == productId);
        }

        public static BaseSpecification<BasketItem> GetBasketItem(Guid basketId)
        {
            var specification =  new BaseSpecification<BasketItem>(bi => bi.BasketId == basketId);
            specification.AddInclude(bi =>  bi.Product);

            return specification;
        }

        public static BaseSpecification<BasketItem> GetListBasketItems(Guid[] itemsId)
        {
            return new BaseSpecification<BasketItem>(c => itemsId.Contains(c.Id));
        }
    }
}
