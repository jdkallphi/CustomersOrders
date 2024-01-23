using CustomersOrders.Classes;
using CustomersOrders.Handlers.Customers.Commands;
using CustomersOrders.Handlers.Customers.Queries;
using CustomersOrders.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CustomersOrders.Handlers.Customers
{
    public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, int>
    {
        private readonly ICustomerRepository _customerRepository;
        public DeleteCustomerCommandHandler(ICustomerRepository customerRepository)
        {

            _customerRepository = customerRepository;
        }
        public Task<int> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = _customerRepository.GetCustomer(request.Id);
            return Task.FromResult(_customerRepository.DeleteCustomer(customer));
        }
    }
}
