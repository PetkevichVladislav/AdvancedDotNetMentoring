using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace IdentityService.SDK.Infrastructure.Middleware
{
    public class JwtLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<JwtLoggingMiddleware> _logger;

        public JwtLoggingMiddleware(RequestDelegate next, ILogger<JwtLoggingMiddleware> logger)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
            {
                var token = authorizationHeader.ToString().Replace("Bearer ", "");
                _logger.LogInformation($"JWT Token: {token}");
            }
            else
            {
                _logger.LogInformation($"Request without JWT Token: {context.Request}");
            }

            await _next(context);
        }
    }
}
