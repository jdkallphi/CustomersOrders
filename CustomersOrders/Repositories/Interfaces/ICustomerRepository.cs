using CustomersOrders.Models.Customers;

namespace CustomersOrders.Repositories
{
    public interface ICustomerRepository
    {
        public Customer AddCustomer(Customer customer);

        public int DeleteCustomer(Customer customer);

        public Customer UpdateCustomer(Customer customer);

        public Customer GetCustomer(int id);

        public IEnumerable<Customer> GetAllCustomers();
        IEnumerable<Customer> SearchCustomers(string mailAddress);
    }
}
