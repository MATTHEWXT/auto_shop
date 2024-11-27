using HieLie.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HieLie.Application.Models.Request
{
    public class CreateOrderReq
    {
        public Guid CustomerId { get; set; }
        public Guid[]? ItemsId { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
