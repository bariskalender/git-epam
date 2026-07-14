using System;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace MiddlewareLibrary.Middleware;

public class RateLimitingMiddleware(
    RequestDelegate next,
    ILogger<RateLimitingMiddleware> logger,
    RateLimitingOptions options)
{
    private static readonly ConcurrentDictionary<string, List<DateTime>> Requests = new();

    public async Task InvokeAsync(HttpContext context)
    {
        var path = context.Request.Path;

        if (ShouldSkip(path))
        {
            await next(context);
            return;
        }

        var clientIp = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        var now = DateTime.UtcNow;
        var windowStart = now.AddMinutes(-options.WindowMinutes);

        var list = Requests.GetOrAdd(clientIp, _ => new List<DateTime>());

        lock (list)
        {
            list.RemoveAll(x => x < windowStart);

            if (list.Count >= options.MaxRequests)
            {
                logger.LogWarning("Rate limit exceeded for {IP}", clientIp);

                context.Response.StatusCode = 429;
                context.Response.Headers["Retry-After"] = "60";
                context.Response.ContentType = "application/json";

                context.Response.WriteAsJsonAsync(new
                {
                    error = "Rate limit exceeded",
                    retryAfter = 60
                }).Wait();

                return;
            }

            list.Add(now);
        }

        await next(context);
    }

    private bool ShouldSkip(PathString path)
    {
        return options.ExcludedPaths.Any(p => path.StartsWithSegments(p));
    }
}
public class RateLimitingOptions
{
    public int MaxRequests { get; set; } = 100;
    public int WindowMinutes { get; set; } = 1;
    public string[] ExcludedPaths { get; set; } = Array.Empty<string>();
}
