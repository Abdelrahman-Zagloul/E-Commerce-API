namespace E_Commerce.Domain.Entities.BasketModule
{
    public class BasketItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string PictureUrl { get; set; } = default!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
