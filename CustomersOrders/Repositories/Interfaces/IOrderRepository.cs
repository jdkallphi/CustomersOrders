
namespace CustomersOrders.Repositories
{
    public interface IOrderRepository
    {
        IEnumerable<Order> GetAllCancelledOrders(int customerId);
        IEnumerable<Order> GetAllOrders(int customerId);
        Order GetOrderById(int orderId);
        IEnumerable<Order> SearchOrders(IEnumerable<int> customerIds, DateTime startDate, DateTime endDate);
    }
}
