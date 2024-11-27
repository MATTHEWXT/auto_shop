using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HieLie.Application.Models.Request
{
    public record CreateUserReq(
        //[Required]
        //[StringLength(50, MinimumLength = 2)]
        string FirstName,
        //[Required]
        //[EmailAddress(ErrorMessage = "Invalid email format")]
        string Email,
        //[Required]
        //[StringLength(50, MinimumLength = 8)]
        string Password);

}
