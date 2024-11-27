using HieLie.Application.Models.DTOS;
using HieLie.Application.Models.Request;
using HieLie.Application.Models.Response;
using HieLie.Domain.Entities;

namespace HieLie.Application.Interfaces
{
    public interface IUserService
    {
        Task<AuthenticationResponse> RegisterUser(CreateUserReq req);
        Task<CreateUserRes> Create(CreateUserReq req);
        Task DeleteAsync(Guid id);
        Task<IList<User>> GetAll();
        Task<UserDTO> GetUserById(Guid userId);
        Task UpdateAsync(Guid id, string FirstName, string email, string password);
    }
}