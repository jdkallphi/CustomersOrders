using AutoMapper;
using CustomersOrders.Classes;
using CustomersOrders.Handlers.Customers.Queries;
using CustomersOrders.Models.DTO;
using CustomersOrders.Repositories;
using MediatR;

namespace CustomersOrders.Handlers.Customers
{
    public class SearchCustomersHandler : IRequestHandler<SearchCustomersQuery, IEnumerable<CustomerDTO>>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;
        public SearchCustomersHandler(ICustomerRepository customerRepository, IMapper mapper)
        {
            _mapper = mapper;
            _customerRepository = customerRepository;
        }
        public Task<IEnumerable<CustomerDTO>> Handle(SearchCustomersQuery request, CancellationToken cancellationToken)
        {
            var result = _mapper.Map<IEnumerable<CustomerDTO>>(_customerRepository.SearchCustomers(request.MailAddress));
            return Task.FromResult(result);
        }
    }
}
