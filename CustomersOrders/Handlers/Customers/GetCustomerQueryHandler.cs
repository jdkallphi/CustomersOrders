using AutoMapper;
using CustomersOrders.Classes;
using CustomersOrders.Handlers.Customers.Queries;
using CustomersOrders.Models.Customers;
using CustomersOrders.Repositories;
using MediatR;

namespace CustomersOrders.Handlers.Customers
{
    public class GetCustomerQueryHandler : IRequestHandler<GetCustomerQuery, CustomerDTO>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;
        public GetCustomerQueryHandler(ICustomerRepository customerRepository, IMapper mapper)
        {
            _mapper = mapper;
            _customerRepository = customerRepository;
        }
        public Task<CustomerDTO> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
        {
            var result = _mapper.Map<CustomerDTO>(_customerRepository.GetCustomer(request.Id));
                
            return Task.FromResult(result);
        }
    }
}
