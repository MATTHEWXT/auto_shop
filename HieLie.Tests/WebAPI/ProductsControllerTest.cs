using HieLie.Domain.Entities;
using HieLie.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;
using System;
using System.Net.Http.Json;
using Xunit;


namespace HieLie.Tests.WebAPI
{
    public class ProductsControllerTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public ProductsControllerTest(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact] 
        public async Task CreateProduct_RerurnsCreateProduct()
        {
            var request = new
            {
                Name = "Test",
                Price = 100,
                ImagePath = "/images/test.jpg",
                CategoryId = Guid.NewGuid()
            };

            var response = await _client.PostAsJsonAsync("/product/create", request);
            response.EnsureSuccessStatusCode();

            var createdProduct = await response.Content.ReadFromJsonAsync<Product>();

            Assert.NotNull(createdProduct);
            Assert.Equal(request.Name, createdProduct.Name);
        }
    }
}
