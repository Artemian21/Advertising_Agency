using Advertising_Agency.DataAccess.Entities;
using Advertising_Agency.Domain.Enums;

namespace Advertising_Agency.DataAccess.Interfaces
{
    public interface IUserRepository
    {
        Task AddAsync(User user);
        Task DeleteAsync(User user);
        Task<bool> ExistsByEmailAsync(string email);
        Task<bool> ExistsByUsernameAsync(string username);
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByIdAsync(Guid id);
        Task<User?> GetByUsernameAsync(string username);
        Task<IEnumerable<User>> GetUsersByRoleAsync(Role role);
        Task UpdateAsync(User user);
    }
}