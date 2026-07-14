using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.IO.Compression;

namespace MiddlewareLibrary.Middleware
{
    public class CompressionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CompressionMiddleware> _logger;

        public CompressionMiddleware(RequestDelegate next, ILogger<CompressionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var originalBody = context.Response.Body;
            var encoding = context.Request.Headers["Accept-Encoding"].ToString().ToLower();

            try
            {
                if (encoding.Contains("gzip"))
                {
                    context.Response.Headers["Content-Encoding"] = "gzip";
                    context.Response.Body = new GZipStream(originalBody, CompressionLevel.Fastest, true);
                }
                else if (encoding.Contains("deflate"))
                {
                    context.Response.Headers["Content-Encoding"] = "deflate";
                    context.Response.Body = new DeflateStream(originalBody, CompressionLevel.Fastest, true);
                }

                await _next(context);
            }
            finally
            {
                if (context.Response.Body != originalBody)
                {
                    await context.Response.Body.DisposeAsync();
                    context.Response.Body = originalBody;

                    _logger.LogDebug("Response stream compressed for {Path}", context.Request.Path);
                }
            }
        }
    }
}
