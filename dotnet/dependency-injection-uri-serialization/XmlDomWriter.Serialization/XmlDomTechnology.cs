using System;
using System.Collections.Generic;
using System.Xml;
using Serialization;

namespace XmlDomWriter.Serialization
{
    public class XmlDomTechnology : IDataSerializer<Uri>
    {
        private readonly string path;

        public XmlDomTechnology(string path)
        {
            this.path = path;
        }

        public XmlDomTechnology(string path, Microsoft.Extensions.Logging.ILogger<XmlDomTechnology> logger)
        {
            this.path = path;
        }

        public void Serialize(IEnumerable<Uri>? source)
        {
            var document = new XmlDocument();
            var root = document.CreateElement("uriAdresses");
            document.AppendChild(root);

            if (source != null)
            {
                foreach (var uri in source)
                {
                    var item = document.CreateElement("uriAdress");
                    root.AppendChild(item);

                    AppendAttributeElement(document, item, "scheme", "name", uri.Scheme);
                    AppendAttributeElement(document, item, "host", "name", uri.Host);

                    var pathElement = document.CreateElement("path");
                    item.AppendChild(pathElement);

                    foreach (var segment in uri.AbsolutePath.Split('/', StringSplitOptions.RemoveEmptyEntries))
                    {
                        AppendTextElement(document, pathElement, "segment", segment);
                    }

                    AppendQuery(document, item, uri);
                }
            }

            document.Save(this.path);
        }

        private static void AppendQuery(XmlDocument document, XmlElement parent, Uri uri)
        {
            var query = uri.Query.TrimStart('?');

            if (string.IsNullOrWhiteSpace(query))
            {
                return;
            }

            var queryElement = document.CreateElement("query");
            parent.AppendChild(queryElement);

            foreach (var pair in query.Split('&', StringSplitOptions.RemoveEmptyEntries))
            {
                var parts = pair.Split('=', 2);
                var parameter = document.CreateElement("parameter");
                parameter.SetAttribute("key", parts[0]);
                parameter.SetAttribute("value", parts.Length > 1 ? parts[1] : string.Empty);
                queryElement.AppendChild(parameter);
            }
        }

        private static void AppendAttributeElement(XmlDocument document, XmlElement parent, string name, string attribute, string value)
        {
            var element = document.CreateElement(name);
            element.SetAttribute(attribute, value);
            parent.AppendChild(element);
        }

        private static void AppendTextElement(XmlDocument document, XmlElement parent, string name, string value)
        {
            var element = document.CreateElement(name);
            element.InnerText = value;
            parent.AppendChild(element);
        }
    }
}
