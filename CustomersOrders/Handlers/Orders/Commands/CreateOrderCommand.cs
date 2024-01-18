using CustomersOrders.Models.DTO;
using MediatR;

namespace CustomersOrders.Handlers.Orders.Commands
{
    public class CreateOrderCommand : IRequest<int>
    {
        public int CustomerId { get; set; }
        public OrderDTO Order { get; set; }
    }
}
