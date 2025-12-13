namespace E_Commerce.Shared.DTOs.Baskets
{
    public record BasketDto(
        string Id,
        ICollection<BasketItemDto> Items,
        string? PaymentIntentId,
        string? ClientSecret,
        decimal ShippingPrice
    );
}
