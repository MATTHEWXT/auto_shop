using HieLie.Application.Interfaces;
using HieLie.Domain.Core.Repositories;
using HieLie.Domain.Core.Specifications;
using HieLie.Domain.Entities;
using HieLie.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace HieLie.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CacheController : ControllerBase
    {
        private readonly ICacheService _cacheService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ShopDbContext _dbContext;


        public CacheController(ICacheService cacheService, IUnitOfWork unitOfWork, ShopDbContext dbContext)
        {
            _cacheService = cacheService;
            _unitOfWork = unitOfWork;
            _dbContext = dbContext;
        }

        [HttpPost("set")]
        public async Task<IActionResult> Set(string key, string value)
        {
            await _cacheService.SetAsync(key, value);
            return Ok();
        }

        [HttpGet("get")]
        public async Task<ActionResult> Get(string key)
        {
            var value =  await _cacheService.GetAsync(key);

            return Ok(value);
        }

        [HttpDelete]
        public async Task<ActionResult> Remove(string key)
        {
            await _cacheService.RemoveAsync(key);

            return Ok();
        }

        [HttpPost("addCategory")]
        public async Task<IActionResult> addCategory(Guid categoruId, string name)
        {
            await _cacheService.AddCategory(categoruId, name);
            return Ok();
        }

        [HttpGet("getCategory")]
        public async Task<ActionResult> GetCategory(Guid categoruId)
        {
            var value = await _cacheService.GetCategory(categoruId);

            return Ok(value);
        }

        [HttpGet("getProductsByCategory/{categoryName}")]
        public async Task<ActionResult> GetProductsByCategory(string categoryName)
        {
            var category = await _unitOfWork.Repository<Category>().FirstOrDefaultAsync(CategorySpecification.GetCategoryByName(categoryName));

            int countProducts = await _dbContext.Products.Where(p => p.CategoryId == category.Id).CountAsync();

            List<Product> products = await _cacheService.GetProducts(category.Id); 
            if(countProducts > products.Count)
            {
                HttpClient client = new HttpClient();
                var response = await client.GetAsync($"http://localhost:5186/product/{category.Name}");
                var responseBody = await response.Content.ReadAsStringAsync();
                var productsList = JsonSerializer.Deserialize<List<Product>>(responseBody, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return Ok(productsList);
            }

            return Ok(products);
        }
    }
}
