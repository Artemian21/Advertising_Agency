using Advertising_Agency.DataAccess.Entities;
using Advertising_Agency.DataAccess.Interfaces;
using Advertising_Agency.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Advertising_Agency.DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AdvertisingAgencyContext _context;

        public UserRepository(AdvertisingAgencyContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByIdAsync(Guid id) =>
            await _context.Users.FindAsync(id);

        public async Task<IEnumerable<User>> GetAllAsync() =>
            await _context.Users.AsNoTracking().ToListAsync();

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(User user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        // Спеціальний метод - отримати користувачів за роллю
        public async Task<IEnumerable<User>> GetUsersByRoleAsync(Role role) =>
            await _context.Users.Where(u => u.Role == role).ToListAsync();

        // Спеціальний метод - перевірити наявність користувача за username/email
        public async Task<bool> ExistsByUsernameAsync(string username) =>
            await _context.Users.AnyAsync(u => u.Username == username);

        public async Task<User?> GetByUsernameAsync(string username) =>
    await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

        public async Task<bool> ExistsByEmailAsync(string email) =>
            await _context.Users.AnyAsync(u => u.Email == email);
    }
}
