using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace MiddlewareLibrary.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var startTime = DateTime.UtcNow;
            var requestId = Guid.NewGuid().ToString("N")[..8];

            context.Response.Headers["X-Request-ID"] = requestId;

            _logger.LogInformation("[{RequestId}] Starting request processing: {Method} {Path}",
                requestId, context.Request.Method, context.Request.Path);

            await _next(context);

            var duration = (DateTime.UtcNow - startTime).TotalMilliseconds;

            _logger.LogInformation("[{RequestId}] Request processing completed: {StatusCode} in {Duration}ms",
                requestId, context.Response.StatusCode, duration);
        }
    }
}
