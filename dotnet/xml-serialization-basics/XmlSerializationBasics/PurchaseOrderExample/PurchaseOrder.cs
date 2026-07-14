using System.Linq;
using System.Xml.Serialization;

namespace XmlSerializationBasics.PurchaseOrderExample
{
    [XmlRoot("purchase-order", Namespace = "http://www.cpandl.com/purchase-order")]
    public class PurchaseOrder
    {
        [XmlAttribute("ship-cost")]
        public decimal ShipCost { get; set; }

        [XmlAttribute("total-cost")]
        public decimal TotalCost { get; set; }

        [XmlElement("order-date", Order = 1)]
        public string OrderDate { get; set; } = string.Empty;

        [XmlElement("delivery-date", Namespace = "http://www.cpandl.com/delivery-date", Order = 2)]
        public DeliveryDate DeliveryDate { get; set; } = new();

        [XmlArray("items", Namespace = "http://www.cpandl.com/purchase-order-item", Order = 3)]
        [XmlArrayItem("order-item", Namespace = "http://www.cpandl.com/purchase-order-item")]
        public OrderedItem[] OrderedItems { get; set; } = System.Array.Empty<OrderedItem>();

        [XmlElement("destination-address", Namespace = "http://www.cpandl.com/address", Order = 4)]
        public Address ShipTo { get; set; } = new();

        [XmlIgnore]
        public decimal SubTotal { get; set; }

        public decimal CalculateSubTotal()
        {
            this.SubTotal = this.OrderedItems?.Sum(i => i.LineTotal) ?? 0m;
            return this.SubTotal;
        }

        public decimal CalculateTotalCost()
        {
            this.TotalCost = this.CalculateSubTotal() + this.ShipCost;
            return this.TotalCost;
        }
    }
}
