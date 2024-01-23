using AutoMapper;
using CustomersOrders.Handlers.Orders.Queries;
using CustomersOrders.Models.Orders;
using CustomersOrders.Repositories;
using MediatR;
using System.Data;

namespace CustomersOrders.Handlers.Orders
{
    public class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQuery, IEnumerable<OrderDTO>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;
        public GetAllOrdersQueryHandler(IMapper mapper, IOrderRepository repo, ICustomerRepository customerRepository)
        {
            _mapper = mapper;
            _orderRepository = repo;
            _customerRepository = customerRepository;
        }
        public Task<IEnumerable<OrderDTO>> Handle(GetAllOrdersQuery message, CancellationToken cancellationToken)
        {
            var customer = _customerRepository.GetCustomer(message.CustomerId);
            if (customer == null)
            {
                throw new Exception("customer not found");
            }
            var result = _mapper.Map<IEnumerable<OrderDTO>>(_orderRepository.GetAllOrders(message.CustomerId));
            return Task.FromResult(result);
        }
    }
}
