using Advertising_Agency.Domain.Models;

namespace Advertising_Agency.BusinessLogic.Interfaces
{
    public interface IAuthService
    {
        Task<string> AuthenticateAsync(LoginDto loginDto);
        Task<UserDto> RegisterAsync(UserRegistrationDto registrationDto);
    }
}