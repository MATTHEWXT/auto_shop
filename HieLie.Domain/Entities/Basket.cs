using HieLie.Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HieLie.Domain.Entities
{
    public class Basket : BaseEnity
    {
        public Guid Id { get; set; }                       
        public Guid UserId { get; set; }
        public User User { get; set; } = default!;        
        public ICollection<BasketItem>? BasketItems { get; set; }
    }
}
