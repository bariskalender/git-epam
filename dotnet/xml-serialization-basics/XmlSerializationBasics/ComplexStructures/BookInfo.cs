using System.Xml.Serialization;

namespace XmlSerializationBasics.ComplexStructures
{
    [XmlRoot("book-description", Namespace = "http://contoso.com/book-description")]
    public class BookInfo
    {
        [XmlElement("book-title", Order = 1)]
        public BookTitle Title { get; set; } = new();

        [XmlElement("book-price", Order = 2)]
        public BookPrice Price { get; set; } = new();

        [XmlElement("book-genre", Order = 3)]
        public string Genre { get; set; } = string.Empty;

        [XmlElement("book-isbn", Order = 4)]
        public string Isbn { get; set; } = string.Empty;

        [XmlElement("book-publication-date", Order = 5)]
        public BookPublicationDate PublicationDate { get; set; } = new();
    }
}
