namespace E_Commerce.Domain.Entities.BasketModule
{
    public class CustomerBasket
    {
        public string Id { get; set; } = default!;

        public ICollection<BasketItem> Items { get; set; } = [];
    }
}
