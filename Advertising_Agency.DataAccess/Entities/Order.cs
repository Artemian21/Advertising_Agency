using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advertising_Agency.DataAccess.Entities
{
    public class Order
    {
        public Guid OrderId { get; set; }         // PK
        public Guid UserId { get; set; }          // FK
        public User User { get; set; }           // Навігаційна властивість

        public DateTime OrderDate { get; set; }
        public string Status { get; set; }       // Статус замовлення (наприклад: Нове, Виконано, Відмінено)
        public decimal TotalPrice { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();  // Послуги в замовленні
    }
}
