using HieLie.Application.Models.Request;
using HieLie.Application.Services;
using HieLie.Domain.Core.Repositories;
using HieLie.Domain.Entities;
using Moq;
using System;
using Xunit;

namespace HieLie.Tests.Application
{
    public class ProductServiceTest
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly ProductService _productService;

        public ProductServiceTest()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockProductRepository = new Mock<IProductRepository>();
            _productService = new ProductService(
                _mockUnitOfWork.Object,
                _mockProductRepository.Object);
        }

        [Fact]
        public async Task CreateAsync_WithValidRequest_ReturnsProsuct()
        {
            _mockUnitOfWork.Setup(uow => uow.Repository<Product>().AddAsync(It.IsAny<Product>()))
                .ReturnsAsync((Product product) => product);

            var request = new CreateProductReq
            {
                Name = "Test",
                Price = 100m,
                ImagePath = "path/to/image.jpg",
                CategoryId = Guid.NewGuid()
            };

            var result = await _productService.CreateAsync(request);

            Assert.NotNull(result);
            Assert.Equal(result.Name, request.Name);
            Assert.Equal(request.Price, result.Price);
            Assert.Equal(request.ImagePath, result.ImagePath);
            Assert.Equal(request.CategoryId, result.CategoryId);
            _mockUnitOfWork.Verify(uow => uow.Repository<Product>().AddAsync(It.IsAny<Product>()), Times.Once);

        }

        [Fact]
        public async Task CreateAsync_WithNullImagePath_ThrowsInvalidOperationException()
        {
            _mockUnitOfWork.Setup(uow => uow.Repository<Product>().AddAsync(It.IsAny<Product>()))
                .ReturnsAsync((Product product) => product);

            var request = new CreateProductReq
            {
                Name = "Test",
                Price = 100m,
                ImagePath = null,
                CategoryId = Guid.NewGuid()
            };

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _productService.CreateAsync(request));
            Assert.Equal("Не удалось создать товар, отсутсвет путь изображения", exception.Message);

            _mockUnitOfWork.Verify(uow => uow.Repository<Product>().AddAsync(It.IsAny<Product>()), Times.Never);
        }
    }
}
