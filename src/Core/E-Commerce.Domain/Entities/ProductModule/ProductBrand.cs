namespace E_Commerce.Domain.Entities.ProductModule
{
    public class ProductBrand : BaseEntity<int>
    {
        public string Name { get; set; } = default!;


        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
