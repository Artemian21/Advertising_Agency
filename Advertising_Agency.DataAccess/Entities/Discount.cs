namespace Advertising_Agency.DataAccess.Entities
{
    public class Discount
    {
        public Guid DiscountId { get; set; }      // PK
        public Guid ServiceId { get; set; }       // FK
        public Service Service { get; set; }     // Навігаційна властивість

        public decimal DiscountPercent { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public string Description { get; set; }
    }
}
