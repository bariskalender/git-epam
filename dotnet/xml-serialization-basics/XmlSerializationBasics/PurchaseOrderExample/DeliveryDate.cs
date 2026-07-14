using System.Xml.Serialization;

namespace XmlSerializationBasics.PurchaseOrderExample
{
    [XmlType(Namespace = "http://www.cpandl.com/delivery-date")]
    public class DeliveryDate
    {
        [XmlIgnore]
        public int DeliveryDay { get; set; }

        [XmlIgnore]
        public int DeliveryMonth { get; set; }

        [XmlIgnore]
        public int DeliveryYear { get; set; }

        [XmlAttribute("day")]
        public int Day
        {
            get => this.DeliveryDay;
            set => this.DeliveryDay = value;
        }

        [XmlAttribute("month")]
        public int Month
        {
            get => this.DeliveryMonth;
            set => this.DeliveryMonth = value;
        }

        [XmlElement("year", Order = 1)]
        public int Year
        {
            get => this.DeliveryYear;
            set => this.DeliveryYear = value;
        }
    }
}
