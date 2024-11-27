using HieLie.Application.Interfaces;
using HieLie.Application.Models.Request;
using HieLie.Domain.Entities;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HieLie.Infrastructure.Services
{
    public class RedisCacheService : ICacheService
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly IDatabase _database;

        public RedisCacheService(IConnectionMultiplexer redis)
        {
            _redis = redis;
            _database = redis.GetDatabase();
        }

        public async Task SetAsync(string key, string value, TimeSpan? expiry = null)
        {
            await _database.StringSetAsync(key, value, expiry);
        }

        public async Task<string> GetAsync(string key)
        {
            var value = await _database.StringGetAsync(key);
            return value.HasValue ? value.ToString() : null;
        }

        public async Task RemoveAsync(string key)
        {
            await _database.KeyDeleteAsync(key);
        }

        public async Task AddCategory(Guid categoryId, string categoryName)
        {
            var key = $"category:{categoryId}";

            var hashEntries = new HashEntry[]
            {
                new HashEntry("name", categoryName)
            };

            _database.HashSet(key, hashEntries);
        }

        public async Task<string> GetCategory(Guid categoryId)
        {
            var key = $"category:{categoryId}";

            return _database.HashGet(key, "name");
        }

        public async Task SetProduct(Product product)
        {
            var productKey = $"product:{product.Id}";
            var categoryKey = $"category:{product.CategoryId}";

            var productData = JsonSerializer.Serialize(product);

            await _database.StringSetAsync(productKey, productData, TimeSpan.FromMinutes(60));
            await _database.ListRightPushAsync(categoryKey, productKey);
        }

        public async Task SetListProducts(List<Product> product)
        {
            foreach(var productItem in product)
            {
                try
                {
                    await SetProduct(productItem);
                }
                catch (Exception)
                {
                    throw new Exception("Ошибка при копировании в Redis");
                }
            }
        }

        public async Task<List<Product>> GetProducts(Guid categoryId)
        {
            var categoryKey = $"category:{categoryId}";

            var productKeys = _database.ListRange(categoryKey);

            if (productKeys.Length == 0) {
                return new List<Product>();
            }

            var products = new List<Product>();

            foreach (var productKey in productKeys)
            {
                var productData = await _database.StringGetAsync(productKey.ToString());

                if(!productData.IsNullOrEmpty)
                {
                    var product = JsonSerializer.Deserialize<Product>(productData);
                    products.Add(product);
                }
            }

            return products;
        }
    }
}
