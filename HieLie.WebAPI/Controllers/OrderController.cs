using HieLie.Application.Interfaces;
using HieLie.Application.Models.Request;
using Microsoft.AspNetCore.Mvc;

namespace HieLie.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IBasketService _basketService;


        public OrderController(IOrderService orderService, IBasketService basketService)
        {
            _orderService = orderService;
            _basketService = basketService;
        }

        [HttpPost("create-order")]
        public async Task<ActionResult> CreateOrder([FromBody] CreateOrderReq req)
        {
            try
            {
                if(req.ItemsId == null)
                {
                    throw new InvalidOperationException("Не удалось создать заказ, так как отсутсвуют элементы закакза");
                }
                var basketItems = await _basketService.GetBasketItemsById(req.ItemsId);
            
                var order = await _orderService.CreateOrder(req);

                await _orderService.CreateOrderItems(order.Id, basketItems);

                return Ok();
            }
            catch(InvalidOperationException)
            {
                return Conflict(new { message = "Не удалось создать заказ! Отсутсвуют товары" });
            }
            
        }
    }
}
