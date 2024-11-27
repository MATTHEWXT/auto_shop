using HieLie.Application.Interfaces;
using HieLie.Application.Models.Request;
using HieLie.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace HieLie.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketService _basketService;

        public BasketController(IBasketService basketService)
        {
            _basketService = basketService;
        }

        [HttpPost]
        public async Task<ActionResult> AddToBasket([FromBody] AddToBasketReq req)
        {
            await _basketService.AddToBasket(req);

            return Ok();
        }

        [HttpGet("basketItems")]
        public async Task<ActionResult> GetBasketItems([FromQuery] Guid userId)
        {
            var basketItems = await _basketService.GetBasketItems(userId);

            return Ok(basketItems);
        }

        [HttpGet("checkout")]
        public async Task<ActionResult> GetCustomer([FromQuery] Guid userId)
        {
            var customerDto = await _basketService.GetCustomerByUserId(userId);
            if (customerDto == null)
            {
                return NotFound();
            }
            return Ok(customerDto);
        }

        [HttpPost("checkout/create-customer")]
        public async Task<ActionResult> CreateCustomer([FromBody] CreateCustomerReq req)
        {

            var isExistCustomer = await _basketService.IsExistCustomer(req.UserId);

            if (!isExistCustomer)
            {
                await _basketService.CreateCustomer(req);

                return Ok();
            }
            else
            {
                return Conflict();
            }
        }

        [HttpPut("checkout/update-customer")]
        public async Task<ActionResult> UpdateCustomer([FromBody] CreateCustomerReq req)
        {

            await _basketService.UpdateCustomer(req);

            return Ok();
        }


    }
}
