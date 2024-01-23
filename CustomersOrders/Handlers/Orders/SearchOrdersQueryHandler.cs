using AutoMapper;
using CustomersOrders.Handlers.Orders.Queries;
using CustomersOrders.Models.Orders;
using CustomersOrders.Repositories;
using MediatR;

namespace CustomersOrders.Handlers.Orders
{
    public class SearchOrdersQueryHandler : IRequestHandler<SearchOrdersQuery, IEnumerable<OrderDTO>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        public SearchOrdersQueryHandler(IMapper mapper, IOrderRepository repo)
        {
            _mapper = mapper;
            _orderRepository = repo;
        }
        public Task<IEnumerable<OrderDTO>> Handle(SearchOrdersQuery message, CancellationToken cancellationToken)
        {
            var result = _mapper.Map<IEnumerable<OrderDTO>>(_orderRepository.SearchOrders(message.CustomerIds, message.StartDate, message.EndDate));
            return Task.FromResult(result);
        }
    }
}
