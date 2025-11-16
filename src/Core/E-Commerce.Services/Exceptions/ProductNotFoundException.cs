namespace E_Commerce.Services.Exceptions
{
    internal sealed class ProductNotFoundException(int id) 
        : NotFoundException($"Product with ID:{id} Not Found")
    {
    }
}
