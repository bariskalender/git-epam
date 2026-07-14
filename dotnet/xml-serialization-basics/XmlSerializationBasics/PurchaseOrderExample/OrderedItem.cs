using System.Xml.Serialization;

namespace XmlSerializationBasics.PurchaseOrderExample
{
    [XmlType(Namespace = "http://www.cpandl.com/purchase-order-item")]
    public class OrderedItem
    {
        [XmlAttribute("unit-price")]
        public decimal UnitPrice { get; set; }

        [XmlAttribute("quantity")]
        public int Quantity { get; set; }

        [XmlElement("order-item-name", Order = 1)]
        public string ItemName { get; set; } = string.Empty;

        [XmlElement("order-item-description", Order = 2)]
        public string Description { get; set; } = string.Empty;

        [XmlIgnore]
        public decimal LineTotal { get; set; }

        public decimal CalculateLineTotal()
        {
            this.LineTotal = this.UnitPrice * this.Quantity;
            return this.LineTotal;
        }
    }
}
