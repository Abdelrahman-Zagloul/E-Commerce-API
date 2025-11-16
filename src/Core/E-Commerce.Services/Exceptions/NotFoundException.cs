namespace E_Commerce.Services.Exceptions
{
    public abstract class NotFoundException(string message) : Exception(message)
    {
    }
}
