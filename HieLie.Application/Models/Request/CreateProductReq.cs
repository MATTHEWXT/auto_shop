
namespace HieLie.Application.Models.Request
{
    public class CreateProductReq
    {
        public string Name { get; set; } = string.Empty;
        public Guid CategoryId { get; set; }
        public decimal Price { get; set; }
        public string? ImagePath { get; set; }
    }
}
