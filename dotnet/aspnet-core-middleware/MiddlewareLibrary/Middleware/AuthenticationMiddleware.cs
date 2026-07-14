using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MiddlewareLibrary.Middleware
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AuthenticationMiddleware> _logger;
        private readonly IConfiguration _configuration;

        public AuthenticationMiddleware(RequestDelegate next, ILogger<AuthenticationMiddleware> logger, IConfiguration configuration)
        {
            _next = next;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (ShouldSkipAuthentication(context.Request.Path))
            {
                await _next(context);
                return;
            }

            if (!context.Request.Headers.TryGetValue("X-API-Key", out var apiKey))
            {
                _logger.LogWarning("API key not provided");
                await ReturnUnauthorized(context, "API key not provided");
                return;
            }

            var validKey = _configuration["ApiKey"];

            if (apiKey != validKey)
            {
                _logger.LogWarning("Invalid API key");
                await ReturnUnauthorized(context, "Invalid API key");
                return;
            }

            _logger.LogInformation("Authentication successful");
            await _next(context);
        }

        private static bool ShouldSkipAuthentication(PathString path)
        {
            return path.StartsWithSegments("/") ||
                   path.StartsWithSegments("/health") ||
                   path.StartsWithSegments("/swagger");
        }

        private static async Task ReturnUnauthorized(HttpContext context, string message)
        {
            context.Response.StatusCode = 401;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsJsonAsync(new
            {
                error = "Authentication error",
                message,
                timestamp = DateTime.UtcNow
            });
        }
    }
}
