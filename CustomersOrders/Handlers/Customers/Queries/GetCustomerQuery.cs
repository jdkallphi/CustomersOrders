using CustomersOrders.Classes;
using CustomersOrders.Models.DTO;
using MediatR;

namespace CustomersOrders.Handlers.Customers.Queries
{
    public class GetCustomerQuery : IRequest<CustomerDTO>
    {
        public int Id { get; set; }
    }
}
