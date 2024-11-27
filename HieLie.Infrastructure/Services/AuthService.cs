using HieLie.Application.Core.Models;
using HieLie.Domain.Entities;
using HieLie.Application.Models.Response;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using HieLie.Domain.Core.Repositories;
using HieLie.Domain.Core.Specifications;
using HieLie.Application.Core.Services;
using HieLie.Application.Models.Request;
using HieLie.Infrastructure.Data;

namespace HieLie.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordService _passwordService;
        private readonly ShopDbContext _dbContext;

        public AuthService(JwtSettings jwtSettings, IUnitOfWork unitOfWork, IPasswordService passwordService, ShopDbContext dbContext)
        {
            _jwtSettings = jwtSettings;
            _unitOfWork = unitOfWork;
            _passwordService = passwordService;
            _dbContext = dbContext;
        }

        public async Task<AuthenticationResponse> Login(LoginRequest req)
        {

            try
            {
                var user = await _unitOfWork.Repository<User>()
                .FirstOrDefaultAsync(UserSpecification.GetUserByEmail(req.Email));

                var result = _passwordService.VerifyPassword(req.Password, user.Password);
                if (result == false)
                {
                    throw new Exception("Неверный пароль");
                }

                return await Authenticate(user.Id, user.Email, req.RefreshToken, user.Role);
            }
            catch (InvalidOperationException)
            {
                throw new Exception("Пользователь с таким Email не найден");
            }
        }

        public bool ValidateAccesToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key)),
                ValidateIssuer = true,
                ValidIssuer = _jwtSettings.Issuer,
                ValidateAudience = true,
                ValidAudience = _jwtSettings.Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

                return true;
            }

            catch (Exception)
            {
                return false;
            }
        }

        public async Task<AuthenticationResponse> ValidateRefreshToken(string token)
        {
            var user = await _unitOfWork.Repository<User>().FirstOrDefaultAsync(UserSpecification.GetUserByRefreshToken(token));

            var userRefreshToken = user?.RefreshTokens?.FirstOrDefault(t => t.Token == token);
            var userRole = user?.Role?.ToString();

            if (user == null || userRefreshToken == null || !userRefreshToken.IsActive)
            {
                throw new UnauthorizedAccessException("Invalid refresh token");
            }

            if (userRole == null)
            {
                throw new InvalidOperationException("Не удалось проверить токены");
            }
            var newTokens = await Authenticate(user.Id, user.Email, token, userRole);

            return newTokens;
        }

        public async Task<AuthenticationResponse> Authenticate(Guid userId, string email, string oldRefreshToken, string userRole)
        {
            var user = await _unitOfWork.Repository<User>().FirstOrDefaultAsync(UserSpecification.GetUserWithRefreshTokens(userId));
            var accsesToken = GenerateJwtToken(userId.ToString(), email, userRole);
            var newRefreshToken = GenerateRefreashToken();

            try
            {
                var existingToken = await _unitOfWork.Repository<RefreshToken>().FirstOrDefaultAsync(UserSpecification.GetRefreshToken(oldRefreshToken));

                if (existingToken.IsActive)
                {
                    newRefreshToken.Expires = existingToken.Expires;
                }

                _dbContext.Remove(existingToken);

                return new AuthenticationResponse
                {
                    AccesToken = accsesToken,
                    RefreshToken = newRefreshToken.Token
                };
            }
            catch (InvalidOperationException)
            {
                user.RefreshTokens.Add(newRefreshToken);
                _dbContext.Update(user);
                _dbContext.SaveChanges();

                return new AuthenticationResponse
                {
                    AccesToken = accsesToken,
                    RefreshToken = newRefreshToken.Token
                };
            }
        }

        private string GenerateJwtToken(string userId, string email, string userRole)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtSettings.Key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, userRole)
            }),

                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiresInMinutes),
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private RefreshToken GenerateRefreashToken()
        {
            var rndBytes = new byte[64];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(rndBytes);

                return new RefreshToken
                {
                    Token = Convert.ToBase64String(rndBytes),
                    Expires = DateTime.UtcNow.AddDays(9),
                    Created = DateTime.UtcNow
                };
            }
        }
    }
}
