using AutoMapper;
using CustomersOrders.Handlers.Orders.Commands;
using CustomersOrders.Repositories;
using MediatR;

namespace CustomersOrders.Handlers.Orders
{
    public class CancelOrderCommandHandler : IRequestHandler<CancelOrderCommand, int>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CancelOrderCommandHandler(IMapper mapper, IOrderRepository repo, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _orderRepository = repo;
            _unitOfWork = unitOfWork;
        }
        public Task<int> Handle(CancelOrderCommand message, CancellationToken cancellationToken)
        {
            var order = _orderRepository.GetOrderById(message.OrderId);
            if (order == null)
            {
                throw new Exception("Order not found");
            }
            order.SetIsCancelled();
            _unitOfWork.SaveChanges();
            return Task.FromResult(order.Id);
        }
    }
}
