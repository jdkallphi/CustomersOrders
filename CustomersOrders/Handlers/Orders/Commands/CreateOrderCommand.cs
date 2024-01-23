using CustomersOrders.Models.Orders;
using MediatR;

namespace CustomersOrders.Handlers.Orders.Commands
{
    public class CreateOrderCommand : IRequest<int>
    {
        public int CustomerId { get; set; }
        public OrderAdd Order { get; set; }
    }
}
