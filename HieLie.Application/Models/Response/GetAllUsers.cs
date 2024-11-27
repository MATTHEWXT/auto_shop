using HieLie.Application.Models.DTOS;
using HieLie.Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HieLie.Application.Models.Response
{
   public class GetAllUsers
    {
        public IList<UserDTO>? Data { get; set; }
    }
}
