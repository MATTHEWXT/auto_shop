using HieLie.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using HieLie.Application.Models.Response;
using HieLie.Application.Models.Request;
using HieLie.Application.Models.DTOS;
using Microsoft.AspNetCore.Authorization;
namespace HieLie.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<ActionResult<CreateUserRes>> Create([FromBody] CreateUserReq req)
        {
            var resault = await _userService.Create(req);
            return Ok(resault);
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<ActionResult<IList<UserDTO>>> GetListAll()
        {
            var list = await _userService.GetAll();
            return Ok(list);
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<UserDTO>> GetUser(Guid userId)
        {
            return await _userService.GetUserById(userId);
        }

        [Authorize(Roles = "admin")]
        [HttpDelete]
        public async Task<ActionResult> Delete([FromBody] Guid id)
        {
            await _userService.DeleteAsync(id);
            return Ok();
        }

        [Authorize(Roles = "admin")]
        [HttpPut]
        public async Task<ActionResult> Update(Guid id, UserUpdateRequest req)
        {
            await _userService.UpdateAsync(id, req.FirstName, req.Email, req.Password);
            return Ok();
        }
    }
}
