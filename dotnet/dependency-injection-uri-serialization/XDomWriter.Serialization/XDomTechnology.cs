using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Serialization;

namespace XDomWriter.Serialization
{
    public class XDomTechnology : IDataSerializer<Uri>
    {
        private readonly string path;

        public XDomTechnology(string path)
        {
            this.path = path;
        }

        public XDomTechnology(string path, Microsoft.Extensions.Logging.ILogger<XDomTechnology> logger)
        {
            this.path = path;
        }

        public void Serialize(IEnumerable<Uri>? source)
        {
            var root = new XElement("uriAdresses", source?.Select(CreateElement));
            root.Save(this.path);
        }

        private static XElement CreateElement(Uri uri)
        {
            var element = new XElement(
                "uriAdress",
                new XElement("scheme", new XAttribute("name", uri.Scheme)),
                new XElement("host", new XAttribute("name", uri.Host)),
                new XElement(
                    "path",
                    uri.AbsolutePath
                        .Split('/', StringSplitOptions.RemoveEmptyEntries)
                        .Select(segment => new XElement("segment", segment))));

            var query = uri.Query.TrimStart('?');

            if (!string.IsNullOrWhiteSpace(query))
            {
                element.Add(
                    new XElement(
                        "query",
                        query
                            .Split('&', StringSplitOptions.RemoveEmptyEntries)
                            .Select(pair => pair.Split('=', 2))
                            .Select(parts => new XElement(
                                "parameter",
                                new XAttribute("key", parts[0]),
                                new XAttribute("value", parts.Length > 1 ? parts[1] : string.Empty)))));
            }

            return element;
        }
    }
}
