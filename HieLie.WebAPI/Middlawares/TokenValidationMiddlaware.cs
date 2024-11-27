using HieLie.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;

namespace HieLie.WebAPI.Middlawares
{
    public class TokenValidationMiddlaware
    {
        private readonly RequestDelegate _next;

        public TokenValidationMiddlaware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IAuthService authService)
        {
            var endPoint = context.GetEndpoint();
            if(endPoint?.Metadata.GetMetadata<IAuthorizeData>() != null)
            {
                var accesToken = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                if (string.IsNullOrEmpty(accesToken) || !authService.ValidateAccesToken(accesToken))
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Invalid access token");
                    return;
                }
            }

            await _next(context);
        }
    }
}
