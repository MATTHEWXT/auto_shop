using Xunit;
using Moq;
using HieLie.Application.Core.Services;
using HieLie.Domain.Core.Repositories;
using HieLie.Infrastructure.Services;
using HieLie.Application.Services;
using HieLie.Application.Models.Request;
using HieLie.Domain.Entities;
using HieLie.Domain.Core.Specifications;
using HieLie.Application.Models.DTOS;
using HieLie.Application.Models.Response;


namespace HieLie.Tests.Application
{
    public class UserServiceTest
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IPasswordService> _mockPasswordService;
        private readonly Mock<IAuthService> _mockAuthService;
        private readonly UserService _userService;

        public UserServiceTest()
        {
            _mockAuthService = new Mock<IAuthService>();
            _mockPasswordService = new Mock<IPasswordService>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();

            _userService = new UserService(
                _mockUnitOfWork.Object,
                _mockPasswordService.Object,
                _mockAuthService.Object
                );

        }

        [Fact]
        public async Task RegisterUser_WhenUserExists_ThrowsException()
        {
            var request = new CreateUserReq(
                "John",
                "existinguser@example.com",
                "Password123");

            _mockUnitOfWork.Setup(uow => uow.Repository<User>()
            .FirstOrDefaultAsync(It.IsAny<IBaseSpecification<User>>()))
            .ReturnsAsync(new User("John", "john.doe@example.com", "password123", "user"));

            var exeption = await Assert.ThrowsAsync<Exception>(() => _userService.RegisterUser(request));


            Assert.Equal("Пользователь с таким Email уже существует!", exeption.Message);
        }

        [Fact]
        public async Task RegisterUser_WhenUserDoesNotExist_ReturnsAuthenticationResponse()
        {
            _mockUnitOfWork.Setup(uow => uow.Repository<User>()
                .FirstOrDefaultAsync(It.IsAny<IBaseSpecification<User>>()))
                .ReturnsAsync((User)null);

            _mockAuthService.Setup(auth => auth.Authenticate(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                           .ReturnsAsync(new AuthenticationResponse { AccesToken = "accse", RefreshToken = "refresh" });

            _mockPasswordService.Setup(ps => ps.GenerateHashPassword(It.IsAny<string>()))
                               .Returns("hashed_password");

            var createUserReq = new CreateUserReq("John", "test@example.com", "Password123!");

            var result = await _userService.RegisterUser(createUserReq);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("accse", result.AccesToken);
            Assert.Equal("refresh", result.RefreshToken);
        }

    }
}
