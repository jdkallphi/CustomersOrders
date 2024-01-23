using CustomersOrders.Classes;
using CustomersOrders.Models.Customers;
using MediatR;

namespace CustomersOrders.Handlers.Customers.Queries
{
    public class GetCustomerQuery : IRequest<CustomerDTO>
    {
        public int Id { get; set; }
    }
}
