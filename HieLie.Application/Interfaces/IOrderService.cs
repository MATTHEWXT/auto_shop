using HieLie.Application.Models.Request;
using HieLie.Domain.Entities;

namespace HieLie.Application.Interfaces
{
    public interface IOrderService
    {
        Task<Order> CreateOrder(CreateOrderReq req);
        Task CreateOrderItems(Guid orderId, IList<BasketItem> basketItems);
        Task<IList<Order>> GetUserOrders(Guid custId);
        Task<IList<OrderItem>> GetOrderItems(Guid orderId);
        Task<IList<Order>> GetAllOrders();
        Task UpdateOrderStatus(Guid orderId, string status);
    }
}