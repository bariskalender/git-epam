using System.Xml.Serialization;

namespace XmlSerializationBasics.SerializationWithXmlElement
{
    [XmlRoot(ElementName = "book")]
    public class BookInfo
    {
        [XmlElement(ElementName = "book-title")]
        public string Title { get; set; } = string.Empty;

        [XmlElement(ElementName = "book-price")]
        public decimal Price { get; set; }

        [XmlElement(ElementName = "book-genre")]
        public string Genre { get; set; } = string.Empty;

        [XmlElement(ElementName = "book-isbn")]
        public string Isbn { get; set; } = string.Empty;

        [XmlElement(ElementName = "book-publication-date")]
        public string PublicationDate { get; set; } = string.Empty;
    }
}
