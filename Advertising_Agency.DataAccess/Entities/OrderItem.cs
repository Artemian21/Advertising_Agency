using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advertising_Agency.DataAccess.Entities
{
    public class OrderItem
    {
        public Guid OrderItemId { get; set; }     // PK
        public Guid OrderId { get; set; }         // FK
        public Order Order { get; set; }         // Навігаційна властивість

        public Guid ServiceId { get; set; }       // FK
        public Service Service { get; set; }     // Навігаційна властивість

        public int Quantity { get; set; }
        public decimal PriceAtOrder { get; set; }  // Ціна на момент замовлення
    }
}
