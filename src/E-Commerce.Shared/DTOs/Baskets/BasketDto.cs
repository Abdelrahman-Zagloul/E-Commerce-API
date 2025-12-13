namespace E_Commerce.Shared.DTOs.Baskets
{
    public class BasketDto
    {
        public string Id { get; init; } = default!;
        public ICollection<BasketItemDto> Items { get; init; } = default!;
        public string? PaymentIntentId { get; init; }
        public string? ClientSecret { get; init; }
        public int? DeliveryMethodId { get; init; }
        public decimal ShippingPrice { get; init; }
    }
}
