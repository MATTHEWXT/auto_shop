using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HieLie.Application.Models.Response
{
    public class AuthenticationResponse
    {
        public string AccesToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
}
