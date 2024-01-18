using CustomersOrders.Classes;
using MediatR;

namespace CustomersOrders.Handlers.Customers.Commands
{
    public class DeleteCustomerCommand : IRequest<int>
    {
        public int Id { get; set; }
    }
}
