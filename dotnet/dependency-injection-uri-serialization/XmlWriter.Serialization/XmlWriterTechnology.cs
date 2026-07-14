using System;
using System.Collections.Generic;
using System.Xml;
using Serialization;

namespace XmlWriter.Serialization
{
    public class XmlWriterTechnology : IDataSerializer<Uri>
    {
        private readonly string path;

        public XmlWriterTechnology(string path)
        {
            this.path = path;
        }

        public XmlWriterTechnology(string path, Microsoft.Extensions.Logging.ILogger<XmlWriterTechnology> logger)
        {
            this.path = path;
        }

        public void Serialize(IEnumerable<Uri>? source)
        {
            using var writer = System.Xml.XmlWriter.Create(this.path, new XmlWriterSettings { Indent = true });

            writer.WriteStartDocument();
            writer.WriteStartElement("uriAdresses");

            if (source != null)
            {
                foreach (var uri in source)
                {
                    writer.WriteStartElement("uriAdress");

                    writer.WriteStartElement("scheme");
                    writer.WriteAttributeString("name", uri.Scheme);
                    writer.WriteEndElement();

                    writer.WriteStartElement("host");
                    writer.WriteAttributeString("name", uri.Host);
                    writer.WriteEndElement();

                    writer.WriteStartElement("path");
                    foreach (var segment in uri.AbsolutePath.Split('/', StringSplitOptions.RemoveEmptyEntries))
                    {
                        writer.WriteElementString("segment", segment);
                    }

                    writer.WriteEndElement();

                    WriteQuery(writer, uri);

                    writer.WriteEndElement();
                }
            }

            writer.WriteEndElement();
            writer.WriteEndDocument();
        }

        private static void WriteQuery(System.Xml.XmlWriter writer, Uri uri)
        {
            var query = uri.Query.TrimStart('?');

            if (string.IsNullOrWhiteSpace(query))
            {
                return;
            }

            writer.WriteStartElement("query");

            foreach (var pair in query.Split('&', StringSplitOptions.RemoveEmptyEntries))
            {
                var parts = pair.Split('=', 2);

                writer.WriteStartElement("parameter");
                writer.WriteAttributeString("key", parts[0]);
                writer.WriteAttributeString("value", parts.Length > 1 ? parts[1] : string.Empty);
                writer.WriteEndElement();
            }

            writer.WriteEndElement();
        }
    }
}
