namespace StoreBLL.Models
{
    using System.Collections.Generic;

    public class CustomerOrderModel
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string OperationTime { get; set; } = string.Empty;

        public int OrderStateId { get; set; }

        public List<OrderDetailModel> Details { get; set; } = new List<OrderDetailModel>();
    }
}