
namespace CustomersOrders.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private AppDbContext _context;
        public OrderRepository(AppDbContext context)
        {
            _context = context;
        }


        public IEnumerable<Order> GetAllCancelledOrders(int customerId)
        {
            return _context.Orders.Where(o=>o.CustomerId == customerId && o.IsCancelled == true).ToList();
        }

        public IEnumerable<Order> GetAllOrders(int customerId)
        {
            return _context.Orders.Where(o=>o.CustomerId == customerId).ToList();
        }

        public Order GetOrderById(int orderId)
        {
            return _context.Orders.FirstOrDefault(o=>o.Id == orderId);
        }

        public IEnumerable<Order> SearchOrders(IEnumerable<int> customerIds, DateTime startDate, DateTime endDate)
        {
            return _context.Orders.Where(o=>customerIds.Contains(o.CustomerId) && o.CreationDate >= startDate && o.CreationDate <= endDate).ToList();
        }
    }
}
