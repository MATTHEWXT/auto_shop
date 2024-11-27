using HieLie.Application.Interfaces;
using HieLie.Application.Models.DTOS;
using HieLie.Application.Models.Request;
using HieLie.Domain.Core.Repositories;
using HieLie.Domain.Core.Specifications;
using HieLie.Domain.Entities;

namespace HieLie.Application.Services
{
    public class BasketService : IBasketService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BasketService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddToBasket(AddToBasketReq req)
        {
            try
            {
                var basket = await _unitOfWork.Repository<Basket>()
                .FirstOrDefaultAsync(BasketSpecification
                .IsBasketByUserId(req.UserId));

                try
                {
                    var basketItem = await _unitOfWork.Repository<BasketItem>()
                .FirstOrDefaultAsync(BasketItemSpecification
                .IsBasketItem(basket.Id, req.ProductId));
                }
                catch (InvalidOperationException)
                {
                    await CreateBasketItem(basket.Id, req.ProductId);
                }
            }
            catch (InvalidOperationException)
            {
                var basket = await CreateBasket(req.UserId);
                await CreateBasketItem(basket.Id, req.ProductId);
            }
        }

        public async Task<IList<BasketItem>> GetBasketItems(Guid userId)
        {
            try
            {
                var basket = await _unitOfWork.Repository<Basket>()
                .FirstOrDefaultAsync(BasketSpecification
                .GetBasketByUserId(userId));

                var basketItems = await _unitOfWork.Repository<BasketItem>()
                .ListAsync(BasketItemSpecification
                .GetBasketItem(basket.Id));

                return basketItems;
            }
            catch (InvalidOperationException)
            {
                var basket = await CreateBasket(userId);

                var basketItems = await _unitOfWork.Repository<BasketItem>()
                .ListAsync(BasketItemSpecification
                .GetBasketItem(basket.Id));

                return basketItems;
            }
        }

        public async Task<CustomerDTO> GetCustomerByUserId(Guid userId)
        {
            try
            {
                var customer = await _unitOfWork.Repository<Customer>()
                .FirstOrDefaultAsync(BasketSpecification
                .GetCustomerByUserId(userId));

                var customerDto = new CustomerDTO
                (customer.Id,
                customer.FirstName,
                customer.LastName,
                customer.PhoneNumber,
                customer.Address)
                { };

                return customerDto;
            }
            catch (InvalidOperationException)
            {
                throw new InvalidOperationException();
            }
        }

        public async Task<bool> IsExistCustomer(Guid userId)
        {
            try
            {
                var customer = await _unitOfWork.Repository<Customer>()
               .FirstOrDefaultAsync(BasketSpecification
               .GetCustomerByUserId(userId));

                return true;

            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }

        public async Task<Basket> CreateBasket(Guid userId)
        {
            Basket basket = new Basket { UserId = userId };

            await _unitOfWork.Repository<Basket>().AddAsync(basket);

            return basket;
        }

        public async Task<BasketItem> CreateBasketItem(Guid basketId, Guid productId)
        {
            BasketItem basketItem = new BasketItem
            {
                BasketId = basketId,
                ProductId = productId
            };

            await _unitOfWork.Repository<BasketItem>().AddAsync(basketItem);

            return basketItem;
        }

        public async Task CreateCustomer(CreateCustomerReq req)
        {
            Customer customer = new Customer(req.FirstName, req.LastName, req.PhoneNumber, req.Address, req.UserId) { };
            customer.Validate();

            await _unitOfWork.Repository<Customer>().AddAsync(customer);
        }

        public async Task UpdateCustomer(CreateCustomerReq req)
        {
            var existingCustomer = await _unitOfWork.Repository<Customer>()
                .FirstOrDefaultAsync(BasketSpecification
                .GetCustomerByUserId(req.UserId));

            existingCustomer.FirstName = req.FirstName;
            existingCustomer.LastName = req.LastName;
            existingCustomer.PhoneNumber = req.PhoneNumber;
            existingCustomer.Address = req.Address;

            existingCustomer.Validate();

            _unitOfWork.Repository<Customer>().Update(existingCustomer);
        }

        public async Task<IList<BasketItem>> GetBasketItemsById(Guid[] itemstId)
        {
            try
            {
                var basketItems = await _unitOfWork.Repository<BasketItem>()
                .ListAsync(BasketItemSpecification.GetListBasketItems(itemstId));

                return basketItems;
            }
            catch (InvalidOperationException)
            {
                throw new InvalidOperationException();
            }
        }
    }
}
