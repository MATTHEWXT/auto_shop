using HieLie.Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HieLie.Domain.Entities
{
    public class Order : BaseEnity
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public DateTime? ShippedDate { get; set; }
        public ICollection<OrderItem>? OrderItems { get; set; }
        public decimal TotalAmount { get; set; }
        public Customer Customer { get; set; } = default!;

        public Order(Guid customerId, decimal totalAmount)
        {
            CustomerId = customerId;
            OrderDate = DateTime.UtcNow;
            Status = "Создано";
            TotalAmount = totalAmount;
        }
        public void Validate()
        {
            if (CustomerId == Guid.Empty)
                throw new ArgumentException("Customer Id can't be undefined");
            else if (string.IsNullOrWhiteSpace(Status))
                throw new ArgumentException("Status is required");
            else if (TotalAmount == 0)
                throw new ArgumentException("Total Amount can't be zero");
        }
    }
}
