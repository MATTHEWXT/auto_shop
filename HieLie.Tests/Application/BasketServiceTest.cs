using HieLie.Application.Models.Request;
using HieLie.Application.Services;
using HieLie.Domain.Core.Repositories;
using HieLie.Domain.Core.Specifications;
using HieLie.Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace HieLie.Tests.Application
{
    public class BasketServiceTest
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly BasketService _basketService;

        public BasketServiceTest()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _basketService = new BasketService(_mockUnitOfWork.Object);
        }

        [Fact]
        public async Task AddToBasket_ShouldCreateBasketItem_WhenBasketAndItemExist()
        {
            var request = new AddToBasketReq()
            {
                ProductId = Guid.NewGuid(),
                UserId = Guid.NewGuid()
            };

            var userExist = new User("f", "email", "hashedPas", "admin");
            var basketExist = new Basket() { Id = Guid.NewGuid(), User = userExist, UserId = request.UserId };
            var basketItemExist = new BasketItem() { Id = Guid.NewGuid(), BasketId = basketExist.Id, ProductId = request.ProductId, UnitPrice = 100 };

            _mockUnitOfWork.Setup(uow => uow.Repository<Basket>().FirstOrDefaultAsync(It.IsAny<IBaseSpecification<Basket>>()))
                .ReturnsAsync(basketExist);

            _mockUnitOfWork.Setup(uow => uow.Repository<BasketItem>().FirstOrDefaultAsync(It.IsAny<IBaseSpecification<BasketItem>>()))
                .ReturnsAsync(basketItemExist);

            await _basketService.AddToBasket(request);

            _mockUnitOfWork.Verify(rep => rep.Repository<BasketItem>().FirstOrDefaultAsync(It.IsAny<IBaseSpecification<BasketItem>>()), Times.Once());
            _mockUnitOfWork.Verify(rep => rep.Repository<Basket>().FirstOrDefaultAsync(It.IsAny<IBaseSpecification<Basket>>()), Times.Once());
            _mockUnitOfWork.Verify(rep => rep.Repository<BasketItem>().AddAsync(It.IsAny<BasketItem>()), Times.Never());
        }

        [Fact]
        public async Task AddToBasket_ShouldCreateBasketAndBasketItem_WhenBasketDoesNotExist()
        {
            var request = new AddToBasketReq()
            {
                ProductId = Guid.NewGuid(),
                UserId = Guid.NewGuid()
            };

            _mockUnitOfWork.Setup(uow => uow.Repository<Basket>().FirstOrDefaultAsync(It.IsAny<IBaseSpecification<Basket>>()))
                 .ReturnsAsync((Basket)null);

            _mockUnitOfWork.Setup(uow => uow.Repository<BasketItem>().FirstOrDefaultAsync(It.IsAny<IBaseSpecification<BasketItem>>()))
                 .ReturnsAsync((BasketItem)null);

            await _basketService.AddToBasket(request);


            _mockUnitOfWork.Verify(uow => uow.Repository<Basket>().AddAsync(It.IsAny<Basket>()), Times.Once());
            _mockUnitOfWork.Verify(uow => uow.Repository<BasketItem>().AddAsync(It.IsAny<BasketItem>()), Times.Once());
        }
    }
}
