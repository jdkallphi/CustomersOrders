using CustomersOrders.Models;
using CustomersOrders.Handlers.Customers.Queries;
using CustomersOrders.Repositories;
using MediatR;
using AutoMapper;
using CustomersOrders.Models.DTO;

namespace CustomersOrders.Handlers.Customers
{
    public class GetAllCustomersHandler : IRequestHandler<GetAllCustomersQuery, IEnumerable<CustomerDTO>>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;
        public GetAllCustomersHandler(ICustomerRepository customerRepository, IMapper mapper)
        {
            _mapper = mapper;
            _customerRepository = customerRepository;
        }
        
        public Task<IEnumerable<CustomerDTO>> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
        {
            var customers = _mapper.Map<IEnumerable<CustomerDTO>>(_customerRepository.GetAllCustomers());
            return Task.FromResult(customers);
        }
    }
}
