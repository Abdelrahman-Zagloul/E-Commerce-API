namespace E_Commerce.Shared.DTOs.Baskets
{
    public record BasketItemDto(int Id, string Name, string PictureUrl, decimal Price, int Quantity);
}
