using MediatR;

namespace CustomersOrders.Handlers.Orders.Commands
{
    public class CancelOrderCommand : IRequest<int>
    {
        public int OrderId { get; set; }
    }
}