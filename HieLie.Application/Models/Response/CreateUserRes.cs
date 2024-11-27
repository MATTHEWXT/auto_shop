using HieLie.Application.Models.DTOS;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HieLie.Application.Models.Response
{
    public class CreateUserRes
    {
        public UserDTO? Data { get; set; }
    }
}
