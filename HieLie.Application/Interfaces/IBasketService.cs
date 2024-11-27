using HieLie.Application.Models.DTOS;
using HieLie.Application.Models.Request;
using HieLie.Domain.Entities;

namespace HieLie.Application.Interfaces
{
    public interface IBasketService
    {
        Task AddToBasket(AddToBasketReq req);
        Task<Basket> CreateBasket(Guid userId);
        Task<BasketItem> CreateBasketItem(Guid basketId, Guid productId);
        Task<IList<BasketItem>> GetBasketItems(Guid userId);
        Task<CustomerDTO> GetCustomerByUserId(Guid userId);
        Task<bool> IsExistCustomer(Guid userId);
        Task CreateCustomer(CreateCustomerReq req);
        Task UpdateCustomer(CreateCustomerReq req);
        Task<IList<BasketItem>> GetBasketItemsById(Guid[] itemstId);
    }
}