using CustomersOrders.Models.DTO;
using MediatR;

namespace CustomersOrders.Handlers.Orders.Queries
{
    public class SearchOrdersQuery : IRequest<IEnumerable<OrderDTO>>
    {
        public IEnumerable<int> CustomerIds { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}