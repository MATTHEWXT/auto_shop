
namespace HieLie.Application.Core.Services
{
    public interface IPasswordService
    {
        public string GenerateHashPassword(string pass);
        bool VerifyPassword(string password, string hashedPassword);
    }
}
