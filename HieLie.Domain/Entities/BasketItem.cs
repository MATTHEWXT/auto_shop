using HieLie.Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HieLie.Domain.Entities
{
    public class BasketItem : BaseEnity
    {
        public Guid Id { get; set; }              
        public Guid BasketId { get; set; }
        [JsonIgnore]
        public Basket Basket { get; set; } = default!;
        public Guid ProductId { get; set; }
        public Product Product { get; set; } = default!;
        public decimal UnitPrice { get; set; }    
    }
}
