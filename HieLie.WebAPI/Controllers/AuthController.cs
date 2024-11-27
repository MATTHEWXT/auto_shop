using HieLie.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using HieLie.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using HieLie.Application.Models.Response;
using HieLie.Application.Models.Request;
using HieLie.Domain.Core.Models;
using HieLie.Application.Models.DTOS;
using Microsoft.AspNetCore.Authentication;
using HieLie.Infrastructure.Services;
using HieLie.Infrastructure.Data;
namespace HieLie.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;

        public AuthController(IUserService userService, IAuthService authService)
        {
            _userService = userService;
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Registeration([FromBody] CreateUserReq req)
        {
            try {
                var tokens = await _userService.RegisterUser(req);

                return Ok(tokens);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginRequest req)
        {
            try
            {
                req.RefreshToken = Request.Headers["Refresh-Token"].ToString();

                var authTokens = await _authService.Login(req);

                return Ok(authTokens);
            }
            catch(Exception ex)
            {
                return Unauthorized(new { message = ex.Message });
            } 
        }

        [HttpPost("refresh-tokens")]
        public async Task<ActionResult> RefreshTokens()
        {
            try
            {
                string refreshToken = Request.Headers["Refresh-Token"].ToString();
                var newTokens = await _authService.ValidateRefreshToken(refreshToken);

                return Ok(newTokens);
            }
            catch(UnauthorizedAccessException)
            {
                return Unauthorized(new { message = "Invalid refresh token" }); 
            }
        }

    }
}
