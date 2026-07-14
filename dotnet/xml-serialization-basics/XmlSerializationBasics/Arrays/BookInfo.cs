using System;
using System.Xml.Serialization;

namespace XmlSerializationBasics.Arrays
{
    [XmlRoot("book-shop-item", Namespace = "http://contoso.com/book-shop-item")]
    public class BookInfo
    {
        [XmlArray("titles", Order = 1)]
        [XmlArrayItem("title")]
        public string[] Titles { get; set; } = Array.Empty<string>();

        [XmlArray("genres", Order = 2)]
        [XmlArrayItem("genre")]
        public string[] Genres { get; set; } = Array.Empty<string>();

        [XmlArray("publication-dates", Order = 3)]
        [XmlArrayItem("publication-date")]
        public string[] PublicationDates { get; set; } = Array.Empty<string>();

        [XmlArray("international-standard-book-numbers", Order = 4)]
        [XmlArrayItem("international-standard-book-number")]
        public string[] Codes { get; set; } = Array.Empty<string>();

        [XmlArray("prices", Order = 5)]
        [XmlArrayItem("price")]
        public decimal[] Prices { get; set; } = Array.Empty<decimal>();
    }
}
