namespace Advertising_Agency.Domain.Models
{
    public class OrderItemDto
    {
        public Guid OrderItemId { get; set; } = Guid.NewGuid(); // PK
        public Guid ServiceId { get; set; }
        public string ServiceName { get; set; }  // Для зручності
        public int Quantity { get; set; }
        public decimal PriceAtOrder { get; set; }
    }
}
