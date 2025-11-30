namespace E_Commerce.Shared.DTOs.Orders
{
    public class DeliveryMethodDto
    {
        public string ShortName { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string DeliveryTime { get; set; } = default!;
        public decimal Cost { get; set; }
    }
}
