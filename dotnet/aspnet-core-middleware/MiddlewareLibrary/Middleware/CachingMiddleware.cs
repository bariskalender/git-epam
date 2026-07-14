using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace MiddlewareLibrary.Middleware
{
    public class CachingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CachingMiddleware> _logger;
        private readonly CachingOptions _options;

        private static readonly ConcurrentDictionary<string, CacheEntry> Cache = new();

        public CachingMiddleware(RequestDelegate next, ILogger<CachingMiddleware> logger, CachingOptions options)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (!ShouldCache(context.Request.Path))
            {
                await _next(context);
                return;
            }

            var key = $"{context.Request.Method}:{context.Request.Path}:{context.Request.QueryString}";

            if (TryGetFromCache(key, out var entry))
            {
                _logger.LogInformation("Cache HIT for {Path}", context.Request.Path);
                await WriteCachedResponse(context, entry);
                return;
            }

            _logger.LogInformation("Cache MISS for {Path}", context.Request.Path);

            var originalBody = context.Response.Body;

            await using var memoryStream = new MemoryStream();
            context.Response.Body = memoryStream;

            await _next(context);

            memoryStream.Position = 0;
            var content = await new StreamReader(memoryStream).ReadToEndAsync();

            var cacheEntry = new CacheEntry
            {
                Content = content,
                ContentType = context.Response.ContentType ?? "application/json",
                StatusCode = context.Response.StatusCode,
                Expiration = DateTime.UtcNow.AddMinutes(_options.CacheDurationMinutes)
            };

            Cache[key] = cacheEntry;

            AddCacheHeaders(context);

            context.Response.Body = originalBody;
            await context.Response.WriteAsync(content);
        }

        private bool ShouldCache(PathString path)
            => _options.CacheablePaths.Any(p => path.StartsWithSegments(p));

        private bool TryGetFromCache(string key, out CacheEntry entry)
        {
            if (Cache.TryGetValue(key, out entry!))
            {
                if (entry.Expiration > DateTime.UtcNow)
                    return true;

                Cache.TryRemove(key, out _);
            }

            entry = default!;
            return false;
        }

        private async Task WriteCachedResponse(HttpContext context, CacheEntry entry)
        {
            context.Response.StatusCode = entry.StatusCode;
            context.Response.ContentType = entry.ContentType;

            AddCacheHeaders(context);

            await context.Response.WriteAsync(entry.Content);
        }

        private static void AddCacheHeaders(HttpContext context)
        {
            context.Response.Headers["Cache-Control"] = "public";
            context.Response.Headers["ETag"] = Guid.NewGuid().ToString();
        }

        private sealed class CacheEntry
        {
            public required string Content { get; init; }
            public required string ContentType { get; init; }
            public int StatusCode { get; init; }
            public DateTime Expiration { get; init; }
        }
    }
}
