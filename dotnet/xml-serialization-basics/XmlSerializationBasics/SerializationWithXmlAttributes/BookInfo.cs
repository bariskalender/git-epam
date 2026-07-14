using System.Xml.Serialization;

namespace XmlSerializationBasics.SerializationWithXmlAttributes
{
    [XmlRoot("book", Namespace = "http://contoso.com/book")]
    public class BookInfo
    {
        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlAttribute("title")]
        public string Title { get; set; } = string.Empty;

        [XmlAttribute("price")]
        public decimal Price { get; set; }

        [XmlAttribute("genre")]
        public string Genre { get; set; } = string.Empty;

        [XmlAttribute("isbn")]
        public string Isbn { get; set; } = string.Empty;

        [XmlAttribute("publication-date")]
        public string PublicationDate { get; set; } = string.Empty;
    }
}
