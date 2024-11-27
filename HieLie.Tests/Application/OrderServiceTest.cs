using HieLie.Application.Models.Request;
using HieLie.Application.Services;
using HieLie.Domain.Core.Repositories;
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
    public class OrderServiceTest
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly OrderService _orderService;

        public OrderServiceTest()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _orderService = new OrderService(_mockUnitOfWork.Object);
        }

        [Fact]
        public async Task CreateOrder_ShouldCreateOrderAndCallAddAsync()
        {
            _mockUnitOfWork.Setup(uow => uow.Repository<Order>().AddAsync(It.IsAny<Order>()))
                .ReturnsAsync((Order order) => order);

            var request = new CreateOrderReq
            {
                CustomerId = Guid.NewGuid(),
                TotalPrice = 100
            };

            var result = await _orderService.CreateOrder(request);

            Assert.NotNull(result);
            Assert.Equal(request.CustomerId, result.CustomerId);
            Assert.Equal(request.TotalPrice, result.TotalAmount);

            _mockUnitOfWork.Verify(rep => rep.Repository<Order>().AddAsync(It.Is<Order>(o => o.CustomerId == request.CustomerId && o.TotalAmount == request.TotalPrice)), Times.Once);
        }
    }
}
