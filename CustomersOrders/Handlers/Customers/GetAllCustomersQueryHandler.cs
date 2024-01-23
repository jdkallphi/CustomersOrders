using CustomersOrders.Models;
using CustomersOrders.Handlers.Customers.Queries;
using CustomersOrders.Repositories;
using MediatR;
using AutoMapper;
using CustomersOrders.Models.Customers;

namespace CustomersOrders.Handlers.Customers
{
    public class GetAllCustomersQueryHandler : IRequestHandler<GetAllCustomersQuery, IEnumerable<CustomerDTO>>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;
        public GetAllCustomersQueryHandler(ICustomerRepository customerRepository, IMapper mapper)
        {
            _mapper = mapper;
            _customerRepository = customerRepository;
        }
        
        public Task<IEnumerable<CustomerDTO>> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
        {
            var repoCustomers = _customerRepository.GetAllCustomers();
            var customers = _mapper.Map<IEnumerable<CustomerDTO>>(repoCustomers);
            return Task.FromResult(customers);
        }
    }
}
