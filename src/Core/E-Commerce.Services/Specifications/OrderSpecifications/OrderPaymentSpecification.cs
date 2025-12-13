using E_Commerce.Domain.Entities.OrderModule;

namespace E_Commerce.Services.Specifications.OrderSpecifications
{

    internal class OrderPaymentSpecification : BaseSpecification<Order, Guid>
    {
        public OrderPaymentSpecification(string paymentIntentId) : base(x => x.PaymentIntentId == paymentIntentId)
        {
        }
    }
}
