using HieLie.Application.Models.Request;
using HieLie.Application.Models.Response;

namespace HieLie.Infrastructure.Services
{
    public interface IAuthService
    {
        Task<AuthenticationResponse> Login(LoginRequest req);
        bool ValidateAccesToken(string token);
        Task<AuthenticationResponse> Authenticate(Guid userId, string email, string oldRefreshToken, string userRole);
        Task<AuthenticationResponse> ValidateRefreshToken(string token);
    }
}