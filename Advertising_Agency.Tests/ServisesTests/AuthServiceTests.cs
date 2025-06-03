using AutoFixture.AutoNSubstitute;
using AutoFixture;
using Castle.Core.Configuration;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Advertising_Agency.DataAccess.Interfaces;
using Advertising_Agency.BusinessLogic.Interfaces;
using Advertising_Agency.Domain.Models;
using Advertising_Agency.BusinessLogic.Services;
using Advertising_Agency.DataAccess.Entities;
using AutoMapper;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;

namespace Advertising_Agency.Tests.ServisesTests
{
    public class AuthServiceTests
    {
        private readonly IFixture _fixture;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly Microsoft.Extensions.Configuration.IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
            _fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _userRepository = _fixture.Freeze<IUserRepository>();
            _passwordHasher = _fixture.Freeze<IPasswordHasher>();
            _mapper = _fixture.Freeze<IMapper>();

            // 🔧 Фіксований правильний IConfiguration
            var configValues = new Dictionary<string, string>
    {
        { "Jwt:Key", "TestSecretKey12345678901234567890" },
        { "Jwt:Issuer", "test-issuer" },
        { "Jwt:Audience", "test-audience" }
    };

            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(configValues)
                .Build();

            _authService = new AuthService(_userRepository, _passwordHasher, _configuration, _mapper);
        }

        [Fact]
        public async Task RegisterAsync_UsernameExists_ThrowsException()
        {
            var dto = _fixture.Create<UserRegistrationDto>();
            _userRepository.ExistsByUsernameAsync(dto.Username).Returns(true);

            await Assert.ThrowsAsync<Exception>(() => _authService.RegisterAsync(dto));
        }

        [Fact]
        public async Task RegisterAsync_EmailExists_ThrowsException()
        {
            var dto = _fixture.Create<UserRegistrationDto>();
            _userRepository.ExistsByUsernameAsync(dto.Username).Returns(false);
            _userRepository.ExistsByEmailAsync(dto.Email).Returns(true);

            await Assert.ThrowsAsync<Exception>(() => _authService.RegisterAsync(dto));
        }

        [Fact]
        public async Task RegisterAsync_ValidData_ReturnsUserDto()
        {
            var dto = _fixture.Create<UserRegistrationDto>();
            var user = _fixture.Build<User>()
                               .Without(x => x.PasswordHash)
                               .Create();

            var userDto = _fixture.Create<UserDto>();
            var hashedPassword = "hashed123";

            _userRepository.ExistsByUsernameAsync(dto.Username).Returns(false);
            _userRepository.ExistsByEmailAsync(dto.Email).Returns(false);
            _mapper.Map<User>(dto).Returns(user);
            _passwordHasher.HashPassword(dto.Password).Returns(hashedPassword);
            _mapper.Map<UserDto>(user).Returns(userDto);

            var result = await _authService.RegisterAsync(dto);

            Assert.Equal(userDto, result);
            Assert.Equal(hashedPassword, user.PasswordHash);
            await _userRepository.Received(1).AddAsync(user);
        }

        [Fact]
        public async Task AuthenticateAsync_UserNotFound_ThrowsException()
        {
            var dto = _fixture.Create<LoginDto>();
            _userRepository.GetByUsernameAsync(dto.Username).Returns((User)null);

            await Assert.ThrowsAsync<Exception>(() => _authService.AuthenticateAsync(dto));
        }

        [Fact]
        public async Task AuthenticateAsync_InvalidPassword_ThrowsException()
        {
            var dto = _fixture.Create<LoginDto>();
            var user = _fixture.Create<User>();
            _userRepository.GetByUsernameAsync(dto.Username).Returns(user);
            _passwordHasher.VerifyPassword(user.PasswordHash, dto.Password).Returns(false);

            await Assert.ThrowsAsync<Exception>(() => _authService.AuthenticateAsync(dto));
        }

        [Fact]
        public async Task AuthenticateAsync_ValidCredentials_ReturnsJwtToken()
        {
            var dto = _fixture.Create<LoginDto>();
            var user = _fixture.Build<User>()
                               .With(u => u.UserId, Guid.NewGuid())
                               .With(u => u.Username, dto.Username)
                               .With(u => u.Email, "test@example.com")
                               .With(u => u.Role, Domain.Enums.Role.RegisteredUser)
                               .Create();

            _userRepository.GetByUsernameAsync(dto.Username).Returns(user);
            _passwordHasher.VerifyPassword(user.PasswordHash, dto.Password).Returns(true);

            var token = await _authService.AuthenticateAsync(dto);

            Assert.False(string.IsNullOrEmpty(token));

            // Перевіримо, що токен дійсно валідний JWT
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            Assert.Equal(user.Username, jwtToken.Claims.First(c => c.Type == ClaimTypes.Name).Value);
        }
    }
}
