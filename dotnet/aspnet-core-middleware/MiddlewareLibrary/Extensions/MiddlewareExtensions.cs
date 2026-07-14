using Microsoft.AspNetCore.Builder;
using MiddlewareLibrary.Middleware;

namespace MiddlewareLibrary.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder app)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));
            return app.UseMiddleware<RequestLoggingMiddleware>();
        }

        public static IApplicationBuilder UseErrorHandling(this IApplicationBuilder app)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));
            return app.UseMiddleware<ErrorHandlingMiddleware>();
        }

        public static IApplicationBuilder UseConfigurableMiddleware(this IApplicationBuilder app, Action<ConfigurableMiddlewareOptions>? configure = null)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));

            var options = new ConfigurableMiddlewareOptions();
            configure?.Invoke(options);

            return app.UseMiddleware<ConfigurableMiddleware>(options);
        }

        public static IApplicationBuilder UseApiAuthentication(this IApplicationBuilder app)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));
            return app.UseMiddleware<AuthenticationMiddleware>();
        }

        public static IApplicationBuilder UseCustomCompression(this IApplicationBuilder app)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));
            return app.UseMiddleware<CompressionMiddleware>();
        }

        public static IApplicationBuilder UseCustomCaching(this IApplicationBuilder app, Action<CachingOptions>? configure = null)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));

            var options = new CachingOptions();
            configure?.Invoke(options);

            return app.UseMiddleware<CachingMiddleware>(options);
        }

        public static IApplicationBuilder UseCustomRateLimiting(this IApplicationBuilder app, Action<RateLimitingOptions>? configure = null)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));

            var options = new RateLimitingOptions();
            configure?.Invoke(options);

            return app.UseMiddleware<RateLimitingMiddleware>(options);
        }
    }
}
