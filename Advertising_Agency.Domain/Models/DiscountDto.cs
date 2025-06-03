namespace Advertising_Agency.Domain.Models
{
    public class DiscountDto
    {
        public Guid DiscountId { get; set; } = Guid.NewGuid();
        public Guid ServiceId { get; set; }
        public decimal DiscountPercent { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public string Description { get; set; }
    }
}
