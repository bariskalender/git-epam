using System;

namespace MiddlewareLibrary.Middleware
{
    public class CachingOptions
    {
        public int CacheDurationMinutes { get; set; } = 60;

        public string[] CacheablePaths { get; set; } = Array.Empty<string>();
    }
}
