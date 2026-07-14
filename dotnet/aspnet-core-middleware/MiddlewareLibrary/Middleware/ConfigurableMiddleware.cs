using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MiddlewareLibrary.Middleware
{
    public class ConfigurableMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ConfigurableMiddlewareOptions _options;

        public ConfigurableMiddleware(RequestDelegate next, ConfigurableMiddlewareOptions options)
        {
            _next = next;
            _options = options;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (_options.EnableLogging)
            {
                var logger = context.RequestServices.GetRequiredService<ILogger<ConfigurableMiddleware>>();
                logger.LogInformation("Request path: {Path}", context.Request.Path);
            }

            if (_options.EnableCustomHeader)
            {
                context.Response.Headers["X-Custom-Header"] = _options.CustomHeaderValue;
            }

            await _next(context);
        }
    }
}
