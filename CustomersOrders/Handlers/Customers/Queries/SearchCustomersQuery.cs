using CustomersOrders.Classes;
using CustomersOrders.Models.DTO;
using MediatR;

namespace CustomersOrders.Handlers.Customers.Queries
{
    public class SearchCustomersQuery: IRequest<IEnumerable<CustomerDTO>>
    {
        public string MailAddress { get; set; }
    }
}
