using HieLie.Application.Models.Request;
using HieLie.Domain.Entities;

namespace HieLie.Application.Interfaces
{
    public interface IProductService
    {
        Task<Product> CreateAsync(CreateProductReq req);
        Task<IList<Product>> GetAll();
        Task<Category> GetAllByCategory(string category);
        Task<IList<Product>> GetAFewProducts();
        Task<Product> GetProductById(Guid productId);
        Task<Product> GetProductByName(string productName);
        Task<IList<Product>> GetProductsBySearchTerm(string searchTerm);
        void UpdateProduct(Product product);
        void DeleteProduct(Product product);
    }
}