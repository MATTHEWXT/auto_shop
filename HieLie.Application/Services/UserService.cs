using HieLie.Application.Core.Services;
using HieLie.Application.Interfaces;
using HieLie.Application.Models.DTOS;
using HieLie.Application.Models.Request;
using HieLie.Application.Models.Response;
using HieLie.Domain.Core.Repositories;
using HieLie.Domain.Core.Specifications;
using HieLie.Domain.Entities;
using HieLie.Infrastructure.Services;

namespace HieLie.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordService _passwordService;
        private readonly IAuthService _authService;

        public UserService(IUnitOfWork unitOfwork, IPasswordService passwordService, IAuthService authService)
        {
            _unitOfWork = unitOfwork;
            _passwordService = passwordService;
            _authService = authService;
        }

        public async Task<AuthenticationResponse> RegisterUser(CreateUserReq req)
        {
            try
            {
                var isExistUser = await _unitOfWork.Repository<User>()
                .FirstOrDefaultAsync(UserSpecification.GetUserByEmail(req.Email));

                throw new Exception("Пользователь с таким Email уже существует!");
            }
            catch (InvalidOperationException)
            {
                var userDTO = await Create(req);

                if (userDTO.Data == null)
                {
                    throw new InvalidOperationException("Ошибка создания пользователя!");
                }

                return await _authService.Authenticate(userDTO.Data.Id, userDTO.Data.Email, string.Empty, userDTO.Data.UserRole);
            }
        }

        public async Task<CreateUserRes> Create(CreateUserReq req)
        {
            string defaultRole = "user";

            var user = new User(req.FirstName, req.Email, req.Password, defaultRole);
            user.Validate();

            user.Password = _passwordService.GenerateHashPassword(req.Password);

            await _unitOfWork.Repository<User>().AddAsync(user);

            return new CreateUserRes { Data = new UserDTO(user) };
        }

        public async Task DeleteAsync(Guid id)
        {
            var user = await _unitOfWork.Repository<User>().GetByIdAsync(id);

            _unitOfWork.Repository<User>().Delete(user);
        }

        public async Task<IList<User>> GetAll()
        {
            return await _unitOfWork.Repository<User>().GetAllAsync();
        }

        public async Task<UserDTO> GetUserById(Guid userId)
        {
            var user = await _unitOfWork.Repository<User>().GetByIdAsync(userId);

            return new UserDTO(user);
        }
        public async Task UpdateAsync(Guid id, string firstName, string email, string password)
        {
            var user = await _unitOfWork.Repository<User>().GetByIdAsync(id);

            user.Update(firstName, email, password);

            _unitOfWork.Repository<User>().Update(user);
        }
    }
}
