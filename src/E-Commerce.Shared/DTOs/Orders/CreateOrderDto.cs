namespace E_Commerce.Shared.DTOs.Orders
{
    public class CreateOrderDto
    {
        public string BasketId { get; set; } = string.Empty;
        public int DeliveryMethodId { get; set; }
        public OrderAddressDto ShippingAddress { get; set; } = default!;
    }
}
