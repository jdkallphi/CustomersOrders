using CustomersOrders.Models.DTO;
using MediatR;

namespace CustomersOrders.Handlers.Orders.Queries
{
    public class GetCancelledOrdersQuery : IRequest<IEnumerable<OrderDTO>>
    {
        public int CustomerId { get; set; }
    }
}