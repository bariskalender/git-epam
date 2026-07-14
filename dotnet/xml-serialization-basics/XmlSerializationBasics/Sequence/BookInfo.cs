using System;
using System.Xml.Serialization;

namespace XmlSerializationBasics.Sequence
{
    [XmlRoot("book-shop-item", Namespace = "http://contoso.com/book-shop-item")]
    public class BookInfo
    {
        [XmlElement("title", Order = 3)]
        public string[] Titles { get; set; } = Array.Empty<string>();

        [XmlElement("price", Order = 4)]
        public decimal[] Prices { get; set; } = Array.Empty<decimal>();

        [XmlElement("genre", Order = 1)]
        public string[] Genres { get; set; } = Array.Empty<string>();

        [XmlElement("international-standard-book-number", Order = 2)]
        public string[] Codes { get; set; } = Array.Empty<string>();

        [XmlElement("publication-date", Order = 5)]
        public string[] PublicationDates { get; set; } = Array.Empty<string>();
    }
}
