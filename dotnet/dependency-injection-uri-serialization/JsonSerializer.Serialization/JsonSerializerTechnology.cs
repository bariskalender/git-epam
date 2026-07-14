using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Serialization;

namespace JsonSerializer.Serialization
{
    public class JsonSerializerTechnology : IDataSerializer<Uri>
    {
        private static readonly JsonSerializerOptions Options = new JsonSerializerOptions
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        };

        private readonly string path;

        public JsonSerializerTechnology(string path)
        {
            this.path = path;
        }

        public JsonSerializerTechnology(string path, Microsoft.Extensions.Logging.ILogger<JsonSerializerTechnology> logger)
        {
            this.path = path;
        }

        public void Serialize(IEnumerable<Uri>? source)
        {
            var data = source?.Select(uri => new
            {
                scheme = uri.Scheme,
                host = uri.Host,
                path = uri.AbsolutePath.Split('/', StringSplitOptions.RemoveEmptyEntries),
                query = GetQuery(uri),
            });

            var json = System.Text.Json.JsonSerializer.Serialize(data, Options);

            File.WriteAllText(this.path, json);
        }

        private static IEnumerable<object>? GetQuery(Uri uri)
        {
            var query = uri.Query.TrimStart('?');

            if (string.IsNullOrWhiteSpace(query))
            {
                return null;
            }

            return query
                .Split('&', StringSplitOptions.RemoveEmptyEntries)
                .Select(q =>
                {
                    var parts = q.Split('=', 2);

                    return new
                    {
                        key = parts[0],
                        value = parts.Length > 1 ? parts[1] : string.Empty,
                    };
                });
        }
    }
}
