using HieLie.Application.Core.Services;
using BC = BCrypt.Net;

namespace HieLie.Infrastructure.Services
{
    public class PasswordService : IPasswordService
    {
        public string GenerateHashPassword(string pass)
        {
            return BC.BCrypt.HashPassword(pass);
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            return BC.BCrypt.Verify(password, hashedPassword);
        }
    }
}
