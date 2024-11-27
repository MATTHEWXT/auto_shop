using HieLie.Application.Interfaces;
using HieLie.Application.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HieLie.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserOrdersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IBasketService _basketService;
        private readonly IOrderService _orderService;

        public UserOrdersController(IUserService userService, IBasketService basketService, IOrderService orderService)
        {
            _userService = userService;
            _basketService = basketService;
            _orderService = orderService;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> GetUserOrders([FromQuery] Guid userId)
        {
            IList<UserOrdersResponse> ordersResponse = new List<UserOrdersResponse>();

            try
            {
                var customer = await _basketService.GetCustomerByUserId(userId);

                var orders = await _orderService.GetUserOrders(customer.Id);

                foreach (var order in orders)
                {
                    var orderItems = await _orderService.GetOrderItems(order.Id);
                    UserOrdersResponse orderResponse = new UserOrdersResponse
                    {
                        Id = order.Id,
                        CustomerId = customer.Id,
                        Date = order.OrderDate,
                        Status = order.Status,
                        ShippedDate = order.ShippedDate,
                        TotalAmount = order.TotalAmount,
                        OrderItems = orderItems
                    };

                    ordersResponse.Add(orderResponse);
                }

                return Ok(ordersResponse);
            }
            catch (InvalidOperationException)
            {
                return Ok(ordersResponse);
            }  
        }
    }
}
