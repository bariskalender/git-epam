using System.Xml.Serialization;

namespace XmlSerializationBasics.ComplexStructures
{
    public class BookPublicationDate
    {
        [XmlAttribute("first-publication")]
        public bool FirstPublication { get; set; }

        // Test Day/Month/Year bekliyor, XML'de publication-day/month/year
        [XmlElement("publication-day", Order = 1)]
        public int Day { get; set; }

        [XmlElement("publication-month", Order = 2)]
        public int Month { get; set; }

        [XmlElement("publication-year", Order = 3)]
        public int Year { get; set; }
    }
}
