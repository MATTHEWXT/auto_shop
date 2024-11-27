using Microsoft.AspNetCore.Mvc;
using HieLie.Application.Models.Request;
using HieLie.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using static System.Runtime.InteropServices.JavaScript.JSType;
using HieLie.Application.Models.DTOS;
using Microsoft.EntityFrameworkCore;

namespace HieLie.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IProductService _productService;
        private readonly ICacheService _cacheService;

        public ProductController(IWebHostEnvironment environment, IProductService productService, ICacheService cacheService)
        {
            _environment = environment;
            _productService = productService;
            _cacheService = cacheService;
        }

        [Authorize(Roles = "admin")]
        [HttpPost("create")]
        public async Task<ActionResult> Create([FromForm] CreateProductReq req, IFormFile imageFile)
        {

            if (imageFile != null && imageFile.Length > 0)
            {
                string fileName = Path.GetFileNameWithoutExtension(imageFile.FileName);
                string extension = Path.GetExtension(imageFile.FileName);
                string newFileName = $"{fileName}_{Guid.NewGuid()}{extension}";
                string uploadPath = Path.Combine(_environment.WebRootPath, "images/products", newFileName);

                using (var fileStream = new FileStream(uploadPath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }

                req.ImagePath = $"/images/products/{newFileName}";
            }

            var product = await _productService.CreateAsync(req);
            await _cacheService.SetProduct(product);

            return Ok();
        }


        [HttpGet("{categoryName}")]
        public async Task<ActionResult> GetProductsByCategory(string categoryName)
        {
            var category = await _productService.GetAllByCategory(categoryName);

            if (category.Products == null)
            {
                throw new InvalidOperationException("Категория не имеет товары");
            }

            _ = Task.Run(async () => await _cacheService.SetListProducts(category.Products));

            return Ok(category.Products);
        }

        [HttpGet("id/{productId}")]
        public async Task<ActionResult> GetProductById(Guid productId)
        {
            var product = await _productService.GetProductById(productId);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpGet("name/{productName}")]
        public async Task<ActionResult> GetProductByName(string productName)
        {
            var product = await _productService.GetProductByName(productName);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpGet("catalog/{searchTerm}")]
        public async Task<IActionResult> SearchProducts(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return BadRequest("Search query cannot be empty.");
            }

            var products = await _productService.GetProductsBySearchTerm(searchTerm);

            return Ok(products);
        }

        [HttpGet("getAFewProductsForHome")]
        public async Task<IActionResult> GetAFewProducts()
        {
            var aFewProducts = await _productService.GetAFewProducts();

            return Ok(aFewProducts);
        }

        [Authorize(Roles = "admin")]
        [HttpPost("update")]
        public async Task<ActionResult> Update([FromForm] UpdateProductReq req, IFormFile? imageFile)
        {

            if (imageFile != null && imageFile.Length > 0)
            {
                string fileName = Path.GetFileNameWithoutExtension(imageFile.FileName);
                string extension = Path.GetExtension(imageFile.FileName);
                string newFileName = $"{fileName}_{Guid.NewGuid()}{extension}";
                string uploadPath = Path.Combine(_environment.WebRootPath, "images/products", newFileName);

                using (var fileStream = new FileStream(uploadPath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }

                req.ImagePath = $"/images/products/{newFileName}";
            }

            var existingProduct = await _productService.GetProductById(req.Id);

            if (existingProduct != null)
            {
                existingProduct.Name = req.Name;
                existingProduct.Price = req.Price;
                existingProduct.CategoryId = req.CategoryId;
                existingProduct.ImagePath = req.ImagePath ?? existingProduct.ImagePath;

                _productService.UpdateProduct(existingProduct);
            }
            return Ok();
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("delete/{producrId}")]
        public async Task<ActionResult> Delete(Guid producrId)
        {
            var product = await _productService.GetProductById(producrId);

            if (product != null)
            {
                _productService.DeleteProduct(product);

                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
    }



}
