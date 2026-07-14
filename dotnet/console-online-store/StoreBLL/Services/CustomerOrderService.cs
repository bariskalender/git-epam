namespace StoreBLL.Services
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using StoreBLL.Models;
    using StoreDAL.Entities;
    using StoreDAL.Interfaces;

    public class CustomerOrderService
    {
        private readonly ICustomerOrderRepository orderRepository;
        private readonly IOrderDetailRepository detailRepository;

        public CustomerOrderService(
            ICustomerOrderRepository orderRepository,
            IOrderDetailRepository detailRepository)
        {
            this.orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            this.detailRepository = detailRepository ?? throw new ArgumentNullException(nameof(detailRepository));
        }

        public void CreateOrder(CustomerOrderModel model)
        {
            if (model == null || model.Details == null)
            {
                return;
            }

            var order = new CustomerOrder
            {
                UserId = model.UserId,
                OperationTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                OrderStateId = 1,
            };

            this.orderRepository.Add(order);

            foreach (var detail in model.Details)
            {
                var orderDetail = new OrderDetail
                {
                    OrderId = order.Id,
                    ProductId = detail.ProductId,
                    Price = detail.Price,
                    ProductAmount = detail.ProductAmount,
                };

                this.detailRepository.Add(orderDetail);
            }
        }

        public IEnumerable<CustomerOrderModel> GetAllOrders()
        {
            return this.orderRepository.GetAll()
                .Select(x => new CustomerOrderModel
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    OperationTime = x.OperationTime,
                    OrderStateId = x.OrderStateId,
                    Details = x.Details.Select(d => new OrderDetailModel
                    {
                        Id = d.Id,
                        OrderId = d.OrderId,
                        ProductId = d.ProductId,
                        Price = d.Price,
                        ProductAmount = d.ProductAmount,
                    }).ToList(),
                })
                .ToList();
        }

        public void CancelOrder(int orderId)
        {
            var order = this.orderRepository.GetById(orderId);

            if (order == null)
            {
                return;
            }

            order.OrderStateId = 2;
            this.orderRepository.Update(order);
        }
    }
}