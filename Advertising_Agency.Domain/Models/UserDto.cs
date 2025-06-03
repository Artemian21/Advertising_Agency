using Advertising_Agency.Domain.Enums;

namespace Advertising_Agency.Domain.Models
{
    public class UserDto
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }
    }
}
