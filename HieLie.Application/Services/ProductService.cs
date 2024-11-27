using HieLie.Application.Interfaces;
using HieLie.Application.Models.Request;
using HieLie.Domain.Core.Repositories;
using HieLie.Domain.Core.Specifications;
using HieLie.Domain.Entities;

namespace HieLie.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductRepository _productRepository;


        public ProductService(IUnitOfWork unitOfWork, IProductRepository productRepository)
        {
            _unitOfWork = unitOfWork;
            _productRepository = productRepository;
        }

        public async Task<Product> CreateAsync(CreateProductReq req)
        {
            if(req.ImagePath == null)
            {
                throw new InvalidOperationException("Не удалось создать товар, отсутсвет путь изображения");
            }
            Product product = new Product(req.Name, req.Price, req.ImagePath, req.CategoryId);
            product.Validate();

            await _unitOfWork.Repository<Product>().AddAsync(product);

            return product;
        }

        public async Task<IList<Product>> GetAll()
        {
            return await _unitOfWork.Repository<Product>().GetAllAsync();
        }
        public async Task<IList<Product>> GetAFewProducts()
        {
            return await _productRepository.GetAFewProducts();
        }

        public async Task<IList<Product>> GetProductsBySearchTerm(string searchTerm)
        {
            return await _unitOfWork.Repository<Product>().ListAsync(ProductSpecification.GetProductsBySearchTerm(searchTerm));
        }
        public async Task<Category> GetAllByCategory(string categoryName)
        {
            return await _unitOfWork.Repository<Category>().FirstOrDefaultAsync(ProductSpecification.GetProductsByCategory(categoryName));
        }

        public async Task<Product> GetProductById(Guid productId)
        {
            return await _unitOfWork.Repository<Product>().GetByIdAsync(productId);
        }

        public async Task<Product> GetProductByName(string productName)
        {
            return await _unitOfWork.Repository<Product>().FirstOrDefaultAsync(ProductSpecification.GetProductByName(productName));
        }

        public void UpdateProduct(Product product)
        {
            _unitOfWork.Repository<Product>().Update(product);
        }

        public void DeleteProduct(Product product)
        {
            _unitOfWork.Repository<Product>().Delete(product);
        }
    }
}
