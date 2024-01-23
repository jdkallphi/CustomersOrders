using AutoMapper;
using CustomersOrders.Classes;
using CustomersOrders.Handlers.Customers.Commands;
using CustomersOrders.Models.Customers;
using CustomersOrders.Repositories;
using FluentValidation;
using MediatR;

namespace CustomersOrders.Handlers.Customers
{
    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, CustomerDTO>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CreateCustomerCommandHandler(ICustomerRepository customerRepository,IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _customerRepository = customerRepository;
            _unitOfWork = unitOfWork;
        }
        public Task<CustomerDTO> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = _mapper.Map<Customer>(request.CustomerAdd);
            var validator = new CustomerValidator(_customerRepository.GetAllCustomers());
            validator.ValidateAndThrow(customer);

            var newCustomer = _customerRepository.AddCustomer(customer);
            _unitOfWork.SaveChanges();

            var result = _mapper.Map<CustomerDTO>(newCustomer);
            return Task.FromResult(result);
        }
    }
}
