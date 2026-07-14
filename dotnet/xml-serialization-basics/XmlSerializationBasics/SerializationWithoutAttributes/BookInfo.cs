namespace XmlSerializationBasics.SerializationWithoutAttributes
{
    public class BookInfo
    {
        public string Title { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public string Genre { get; set; } = string.Empty;

        public string Isbn { get; set; } = string.Empty;

        public string PublicationDate { get; set; } = string.Empty;
    }
}
