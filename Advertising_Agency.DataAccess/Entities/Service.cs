using Advertising_Agency.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advertising_Agency.DataAccess.Entities
{
    public class Service
    {
        public Guid ServiceId { get; set; }      // PK
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public ServiceType ServiceType { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();  // Замовлення з цією послугою

        public ICollection<Discount> Discounts { get; set; } = new List<Discount>();  // Знижки на цю послугу
    }
}
