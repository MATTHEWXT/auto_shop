using HieLie.Application.Interfaces;
using HieLie.Application.Models.Request;
using HieLie.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NickBuhro.Translit;

namespace HieLie.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var categories = await _categoryService.GetAll();

            return Ok(new { categories });
        }

        [Authorize(Roles = "admin")]
        [HttpPost("create")]
        public async Task<ActionResult> Create([FromForm] CreateCategoryReq req)
        {
            try
            {
                await _categoryService.CreateAsync(req);
                return Ok();
            }
            catch (InvalidOperationException)
            {
                return Conflict(new { message = "Категория уже существует"});
            }
        }

    }
}
