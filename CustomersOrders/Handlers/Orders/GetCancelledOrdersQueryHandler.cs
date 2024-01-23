using AutoMapper;
using Azure.Core;
using CustomersOrders.Handlers.Orders.Queries;
using CustomersOrders.Models.Orders;
using CustomersOrders.Repositories;
using MediatR;

namespace CustomersOrders.Handlers.Orders
{
    public class GetCancelledOrdersQueryHandler : IRequestHandler<GetCancelledOrdersQuery, IEnumerable<OrderDTO>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly ICustomerRepository _customerRepository;
        public GetCancelledOrdersQueryHandler(IMapper mapper, IOrderRepository repo, ICustomerRepository customerRepository)
        {
            _mapper = mapper;
            _orderRepository = repo;
            _customerRepository = customerRepository;
        }
        public Task<IEnumerable<OrderDTO>> Handle(GetCancelledOrdersQuery message, CancellationToken cancellationToken)
        {
            var customer = _customerRepository.GetCustomer(message.CustomerId);
            if (customer == null)
            {
                throw new Exception("customer not found");
            }
            var result = _mapper.Map<IEnumerable<OrderDTO>>(_orderRepository.GetAllCancelledOrders(message.CustomerId));
            return Task.FromResult(result);
        }

    }
}
