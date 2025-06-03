using Advertising_Agency.BusinessLogic.Services;
using Advertising_Agency.DataAccess.Entities;
using Advertising_Agency.DataAccess.Interfaces;
using Advertising_Agency.Domain.Models;
using AutoFixture.AutoNSubstitute;
using AutoFixture;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;

namespace Advertising_Agency.Tests.ServisesTests
{
    public class UserServiceTests
    {
        private readonly IFixture _fixture;
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;
        private readonly UserService _sut;

        public UserServiceTests()
        {
            _fixture = new Fixture().Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });
            _fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _repository = _fixture.Freeze<IUserRepository>();
            _mapper = _fixture.Freeze<IMapper>();

            _sut = new UserService(_repository, _mapper);
        }

        [Fact]
        public async Task GetAllUsersAsync_ReturnsMappedUsers()
        {
            var users = _fixture.CreateMany<User>(3);
            var dtos = _fixture.CreateMany<UserDto>(3);

            _repository.GetAllAsync().Returns(users);
            _mapper.Map<IEnumerable<UserDto>>(users).Returns(dtos);

            var result = await _sut.GetAllUsersAsync();

            Assert.Equal(dtos, result);
        }

        [Fact]
        public async Task GetUserByIdAsync_ReturnsMappedUser()
        {
            var user = _fixture.Create<User>();
            var dto = _fixture.Create<UserDto>();

            _repository.GetByIdAsync(user.UserId).Returns(user);
            _mapper.Map<UserDto>(user).Returns(dto);

            var result = await _sut.GetUserByIdAsync(user.UserId);

            Assert.Equal(dto, result);
        }

        [Fact]
        public async Task CreateUserAsync_AddsUserAndReturnsDto()
        {
            var regDto = _fixture.Create<UserRegistrationDto>();
            var user = _fixture.Create<User>();
            var dto = _fixture.Create<UserDto>();
            var passwordHash = "hashedPassword123";

            _mapper.Map<User>(regDto).Returns(user);
            _mapper.Map<UserDto>(user).Returns(dto);

            var result = await _sut.CreateUserAsync(regDto, passwordHash);

            Assert.Equal(passwordHash, user.PasswordHash);
            await _repository.Received(1).AddAsync(user);
            Assert.Equal(dto, result);
        }

        [Fact]
        public async Task UpdateUserAsync_UpdatesUser_WhenUserExists()
        {
            var dto = _fixture.Create<UserDto>();
            var user = _fixture.Create<User>();

            _repository.GetByIdAsync(dto.UserId).Returns(user);

            await _sut.UpdateUserAsync(dto);

            Assert.Equal(dto.Username, user.Username);
            Assert.Equal(dto.Email, user.Email);
            Assert.Equal(dto.Role, user.Role);

            await _repository.Received(1).UpdateAsync(user);
        }

        [Fact]
        public async Task UpdateUserAsync_ThrowsException_WhenUserNotFound()
        {
            var dto = _fixture.Create<UserDto>();
            _repository.GetByIdAsync(dto.UserId).Returns((User)null);

            var act = async () => await _sut.UpdateUserAsync(dto);

            var ex = await Assert.ThrowsAsync<Exception>(act);
            Assert.Equal("User not found", ex.Message);
        }

        [Fact]
        public async Task DeleteUserAsync_DeletesUser_WhenUserExists()
        {
            var user = _fixture.Create<User>();
            _repository.GetByIdAsync(user.UserId).Returns(user);

            await _sut.DeleteUserAsync(user.UserId);

            await _repository.Received(1).DeleteAsync(user);
        }

        [Fact]
        public async Task DeleteUserAsync_ThrowsException_WhenUserNotFound()
        {
            var id = Guid.NewGuid();
            _repository.GetByIdAsync(id).Returns((User)null);

            var act = async () => await _sut.DeleteUserAsync(id);

            var ex = await Assert.ThrowsAsync<Exception>(act);
            Assert.Equal("User not found", ex.Message);
        }
    }
}
