using HieLie.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HieLie.Application.Models.Response
{
    public class UserOrdersResponse
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime? ShippedDate { get; set; }
        public decimal TotalAmount { get; set; }
        public ICollection<OrderItem>? OrderItems { get; set; }
    }
}
