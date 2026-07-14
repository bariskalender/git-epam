using System.Xml.Serialization;

namespace XmlSerializationBasics.PurchaseOrderExample
{
    [XmlType(Namespace = "http://www.cpandl.com/address")]
    public class Address
    {
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlIgnore]
        public string State { get; set; } = string.Empty;

        [XmlAttribute("ship-to-state")]
        public string ShipToState
        {
            get => this.State;
            set => this.State = value;
        }

        [XmlAttribute("zip")]
        public string Zip { get; set; } = string.Empty;

        [XmlElement("ship-to-city", Order = 1)]
        public string City { get; set; } = string.Empty;

        [XmlElement("ship-to-line1", Order = 2)]
        public string Line1 { get; set; } = string.Empty;
    }
}
