using System.ComponentModel.DataAnnotations;
using CustomersOrders.Models.Orders;
namespace CustomersOrders.Models.Customers
{
    public class Customer
    {
        private ICollection<Order> _orders;
        public Customer()
        {
            _orders = new List<Order>();
        }

        [Key]
        public int Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public virtual ICollection<Order> Orders => _orders;
        public int NumberOfOrders { get; set; }

        public void AddOrder(Order order)
        {
            order.SetCustomer(this);
            _orders.Add(order);
            NumberOfOrders++;
        }
        public void RemoveOrder(Order order)
        {
            _orders.Remove(order);
            NumberOfOrders--;
        }
        public void Clear()
        {
            _orders.Clear();
            NumberOfOrders = 0;

        }

        public Customer SetName(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
            return this;
        }
        public Customer SetEmail(string email)
        {
            Email = email;
            return this;
        }
    }
}
