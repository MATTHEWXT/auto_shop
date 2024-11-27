using HieLie.Domain.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace HieLie.Domain.Entities
{
    public class User : BaseEnity
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public List<RefreshToken> RefreshTokens { get; set; }

        public User(string firstName, string  email, string password, string role)
        {
            FirstName = firstName;
            Email = email;
            Password = password;
            RefreshTokens = new List<RefreshToken>();
            Role = role;
        }
        public void Update(string firstName, string email, string password)
        {
            if (!string.IsNullOrWhiteSpace(firstName))
            {
                FirstName = firstName;
            }

            if (!string.IsNullOrWhiteSpace(email))
            {
                Email = email;
            }

            if (!string.IsNullOrWhiteSpace(password))
            {
                Password = password;
            }
        }
        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(FirstName))
                throw new ArgumentException("First name is required.");
            else if (string.IsNullOrWhiteSpace(Email))
                throw new ArgumentException("A valid email is required.");
            else if (string.IsNullOrWhiteSpace(Password) || Password.Length < 6)
                throw new ArgumentException("Password must be at least 6 characters long.");
        }

    }


}
