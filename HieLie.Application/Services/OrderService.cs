using HieLie.Application.Interfaces;
using HieLie.Application.Models.Request;
using HieLie.Domain.Core.Repositories;
using HieLie.Domain.Core.Specifications;
using HieLie.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HieLie.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Order> CreateOrder(CreateOrderReq req)
        {
            Order order = new Order(req.CustomerId, req.TotalPrice) {};
            order.Validate();

            await _unitOfWork.Repository<Order>().AddAsync(order);

            return order;
        }

        public async Task CreateOrderItems(Guid orderId, IList<BasketItem> basketItems)
        {
            var orderItems = basketItems.Select(basketItem => new OrderItem
            {
                OrderId = orderId,
                ProductId = basketItem.ProductId,
                UnitPrice = basketItem.UnitPrice,
            }).ToList();

            await _unitOfWork.Repository<OrderItem>().AddListAsync(orderItems);
            await RemoveBasketItems(basketItems);
        }

        public async Task RemoveBasketItems(IList<BasketItem> basketItems)
        {
            await _unitOfWork.Repository<BasketItem>().DeleteEntitiesAsync(basketItems);
        }

        public async Task<IList<Order>> GetUserOrders(Guid custId)
        {
            var orders = await _unitOfWork.Repository<Order>().ListAsync(UserOrdersSpecification.GetUserOrders(custId));
            return orders;
        }

        public async Task<IList<Order>> GetAllOrders()
        {
            var orders = await _unitOfWork.Repository<Order>().GetAllAsync();

            return orders;
        }

        public async Task<IList<OrderItem>> GetOrderItems(Guid orderId)
        {
            var orderItems = await _unitOfWork.Repository<OrderItem>().ListAsync(UserOrdersSpecification.GetOrderItems(orderId));

            return orderItems;
        }

        public async Task UpdateOrderStatus(Guid orderId, string status)
        {
            var order = await _unitOfWork.Repository<Order>().GetByIdAsync(orderId);
            order.Status = status;
            _unitOfWork.Repository<Order>().Update(order);
        }
    }
}
