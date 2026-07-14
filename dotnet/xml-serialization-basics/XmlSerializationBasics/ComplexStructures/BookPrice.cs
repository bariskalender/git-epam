using System.Xml.Serialization;

namespace XmlSerializationBasics.ComplexStructures
{
    public class BookPrice
    {
        [XmlAttribute("currency")]
        public string Currency { get; set; } = string.Empty;

        [XmlElement("price", Order = 1)]
        public decimal Price { get; set; }
    }
}
