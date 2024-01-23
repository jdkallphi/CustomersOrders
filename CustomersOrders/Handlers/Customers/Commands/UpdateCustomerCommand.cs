using CustomersOrders.Models.Customers;
using MediatR;

namespace CustomersOrders.Handlers.Customers.Commands
{
    public class UpdateCustomerCommand : IRequest<CustomerDTO>
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
