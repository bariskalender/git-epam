using System.Xml.Serialization;

namespace XmlSerializationBasics.FieldsSerialization
{
    [XmlRoot("book.info", Namespace = "http://contoso.com/book-info")]
    public class BookInfo
    {
        [XmlElement("sell.price", Order = 3)]
        public decimal Price;

        [XmlElement("category", Order = 1)]
        public string Genre = string.Empty;

        [XmlIgnore]
        private string? isbn;

        [XmlIgnore]
        private string? publicationDate;

        public BookInfo()
        {
            this.isbn = string.Empty;
            this.publicationDate = string.Empty;

            _ = this.isbn;
            _ = this.publicationDate;
        }

        [XmlElement("book.title", Order = 2)]
        public string Title { get; set; } = string.Empty;
    }
}
