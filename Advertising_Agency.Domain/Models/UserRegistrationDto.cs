using Advertising_Agency.Domain.Enums;

namespace Advertising_Agency.Domain.Models
{
    public class UserRegistrationDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }
    }
}
