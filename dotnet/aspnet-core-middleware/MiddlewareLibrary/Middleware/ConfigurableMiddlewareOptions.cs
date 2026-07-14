using System;

namespace MiddlewareLibrary.Middleware
{
    public class ConfigurableMiddlewareOptions
    {
        public bool EnableLogging { get; set; } = true;

        public bool EnableCustomHeader { get; set; } = false;

        public string CustomHeaderValue { get; set; } = "Custom Value";
    }
}
