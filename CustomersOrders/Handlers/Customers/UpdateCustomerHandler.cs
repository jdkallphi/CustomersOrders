using AutoMapper;
using CustomersOrders.Classes;
using CustomersOrders.Handlers.Customers.Commands;
using CustomersOrders.Models.DTO;
using CustomersOrders.Repositories;
using FluentValidation;
using MediatR;

namespace CustomersOrders.Handlers.Customers
{
    public class UpdateCustomerHandler : IRequestHandler<UpdateCustomerCommand, CustomerDTO>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public UpdateCustomerHandler(ICustomerRepository customerRepository,IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _customerRepository = customerRepository;
            _unitOfWork = unitOfWork;
        }
        public Task<CustomerDTO> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            var existingCustomer = _customerRepository.GetCustomer(request.Id);

            if (existingCustomer == null)
            {
                return Task.FromResult<CustomerDTO>(null);
            }
            var updatedCustomer = CheckValidity(request, existingCustomer);
            UpdateExistingCustomer(existingCustomer, updatedCustomer);

            var result = _customerRepository.UpdateCustomer(existingCustomer);
            _unitOfWork.SaveChanges();

            var customer = _mapper.Map<CustomerDTO>(result);
            return Task.FromResult(customer);
        }

        private Customer CheckValidity(UpdateCustomerCommand request, Customer existingCustomer)
        {
            var customerDTO = new CustomerDTO
            {
                Id = request.Id,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email
            };

            var customer = _mapper.Map<Customer>(customerDTO);

            var validator = new CustomerValidator(_customerRepository.GetAllCustomers());
            validator.ValidateAndThrow(customer);

            return customer;
        }

        private void UpdateExistingCustomer(Customer existingCustomer, Customer updatedCustomer)
        {
            existingCustomer.FirstName = updatedCustomer.FirstName;
            existingCustomer.LastName = updatedCustomer.LastName;
            existingCustomer.Email = updatedCustomer.Email;
        }
    }
}
