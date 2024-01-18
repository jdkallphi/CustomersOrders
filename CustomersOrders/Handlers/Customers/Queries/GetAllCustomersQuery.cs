using CustomersOrders.Models.DTO;
using MediatR;

namespace CustomersOrders.Handlers.Customers.Queries
{
    public class GetAllCustomersQuery: IRequest<IEnumerable<CustomerDTO>>
    {
    }
}
