using CustomersOrders.Models.Customers;
using MediatR;

namespace CustomersOrders.Handlers.Customers.Queries
{
    public class GetAllCustomersQuery: IRequest<IEnumerable<CustomerDTO>>
    {
    }
}
