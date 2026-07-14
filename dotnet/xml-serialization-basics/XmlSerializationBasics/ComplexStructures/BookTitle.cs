using System.Xml.Serialization;

namespace XmlSerializationBasics.ComplexStructures
{
    public class BookTitle
    {
        [XmlAttribute("language")]
        public string Language { get; set; } = string.Empty;

        [XmlElement("text", Order = 1)]
        public string Title { get; set; } = string.Empty;
    }
}
