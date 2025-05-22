using Advertising_Agency.Domain.Models;

namespace Advertising_Agency.BusinessLogic.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> CreateUserAsync(UserRegistrationDto newUserDto, string passwordHash);
        Task DeleteUserAsync(Guid userId);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto> GetUserByIdAsync(Guid userId);
        Task UpdateUserAsync(UserDto userDto);
    }
}