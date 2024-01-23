using CustomersOrders.Classes;
using CustomersOrders.Models.Customers;
using MediatR;

namespace CustomersOrders.Handlers.Customers.Commands
{
    public class CreateCustomerCommand : IRequest<CustomerDTO>
    {
        public CustomerAdd CustomerAdd { get; set; }
    }
}
