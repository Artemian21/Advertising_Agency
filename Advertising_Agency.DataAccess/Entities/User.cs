using Advertising_Agency.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advertising_Agency.DataAccess.Entities
{
    public class User
    {
        public Guid UserId { get; set; }         // PK
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }

        public Role Role { get; set; }

        public ICollection<Order> Orders { get; set; } = new List<Order>();  // Замовлення користувача
    }
}
