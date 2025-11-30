using E_Commerce.Domain.Entities.OrderModule;

namespace E_Commerce.Services.Specifications.OrderSpecifications
{
    internal class OrderSpecification : BaseSpecification<Order, Guid>
    {
        public OrderSpecification(string userEmail) : base(x => x.UserEmail == userEmail)
        {
            AddInclude(o => o.Items);
            AddInclude(o => o.DeliveryMethod);
            AddOrderBy(x => x.OrderDate);
            DisableTracking();
        }

        public OrderSpecification(Guid orderId, string userEmail)
            : base(x => x.Id == orderId && x.UserEmail == userEmail)
        {
            AddInclude(o => o.Items);
            AddInclude(o => o.DeliveryMethod);
            AddOrderBy(x => x.OrderDate);
            DisableTracking();
        }
    }
}
