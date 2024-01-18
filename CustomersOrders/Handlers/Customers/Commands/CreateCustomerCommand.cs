using CustomersOrders.Classes;
using CustomersOrders.Models;
using CustomersOrders.Models.DTO;
using MediatR;

namespace CustomersOrders.Handlers.Customers.Commands
{
    public class CreateCustomerCommand : IRequest<CustomerDTO>
    {
        public CustomerAdd CustomerAdd { get; set; }
    }
}
