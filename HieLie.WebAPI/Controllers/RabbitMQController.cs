using HieLie.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace HieLie.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RabbitMQController : ControllerBase
    {
        private readonly IRabbitMqService _rabbitMqService;
        public RabbitMQController(IRabbitMqService rabbitMqService)
        {
            _rabbitMqService = rabbitMqService;
        }

        [HttpPost("send")]
        public async Task<ActionResult> Publish([FromBody] string message)
        {
            await _rabbitMqService.Publish(message);

            return Ok(new { Message = "Message sent successfully", Content = message });
        }

        [HttpGet("consume")]
        public async Task<ActionResult> Consume()
        {
            await _rabbitMqService.Consume();

            return Ok("Consumer started");
        }
    }
}
