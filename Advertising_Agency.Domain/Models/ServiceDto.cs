using Advertising_Agency.Domain.Enums;

namespace Advertising_Agency.Domain.Models
{
    public class ServiceDto
    {
        public Guid ServiceId { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public ServiceType ServiceType { get; set; }
    }
}
