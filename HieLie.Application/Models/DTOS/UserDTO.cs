using HieLie.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HieLie.Application.Models.DTOS
{
    public class UserDTO
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserRole {  get; set; }
        public UserDTO(User user)
        {
            Id = user.Id;
            FirstName = user.FirstName;
            Email = user.Email;
            Password = user.Password;
            UserRole = user.Role;
        }
    }
}
