using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HieLie.Application.Models.Request
{
    public class AddToBasketReq
    {
        public Guid UserId { get; set; } 
        public Guid ProductId { get; set; } 
    }
}
