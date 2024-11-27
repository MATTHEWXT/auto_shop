using HieLie.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using HieLie.Application.Models.Response;
using HieLie.Application.Models.Request;
using Microsoft.AspNetCore.Authorization;

namespace HieLie.WebAPI.Controllers
{
    [Authorize(Roles = "manager")]
    [ApiController]
    [Route("[controller]")]
    public class ManagerController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IBasketService _basketService;
        private readonly IOrderService _orderService;

        public ManagerController(IUserService userService, IBasketService basketService, IOrderService orderService)
        {
            _userService = userService;
            _basketService = basketService;
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrders();

            IList<UserOrdersResponse> ordersResponse = new List<UserOrdersResponse>();

            foreach (var order in orders)
            {
                var orderItems = await _orderService.GetOrderItems(order.Id);
                UserOrdersResponse orderResponse = new UserOrdersResponse
                {
                    Id = order.Id,
                    CustomerId = order.CustomerId,
                    Date = order.OrderDate,
                    Status = order.Status,
                    ShippedDate = order.ShippedDate,
                    TotalAmount = order.TotalAmount,
                    OrderItems = orderItems,
                };

                ordersResponse.Add(orderResponse);
            }

            return Ok(ordersResponse);
        }

        [HttpPatch("{orderId}/status")]
        public async Task<ActionResult> UpdateOrderStatus(Guid orderId, [FromBody] UpdateStatusRequest req)
        {
            await _orderService.UpdateOrderStatus(orderId, req.Status);

            return Ok();
        }

    }
}
