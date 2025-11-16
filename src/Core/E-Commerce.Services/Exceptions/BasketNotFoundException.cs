namespace E_Commerce.Services.Exceptions
{
    internal sealed class BasketNotFoundException(string id)
        : NotFoundException($"Basket with ID:{id} Not Found")
    {
    }
}
