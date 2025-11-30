namespace E_Commerce.Shared.DTOs.Orders
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public string UserEmail { get; set; } = string.Empty;
        public DateTimeOffset OrderDate { get; set; }
        public OrderAddressDto ShippingAddress { get; set; } = default!;
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }
        public string DeliveryMethod { get; set; } = default!;

        public ICollection<OrderItemDto> Items { get; set; } = [];
    }
}
