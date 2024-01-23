using CustomersOrders.Models.Customers;
using Microsoft.EntityFrameworkCore;

namespace CustomersOrders.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private AppDbContext _context;
        public CustomerRepository(AppDbContext context) {
            _context = context;
        }

        public Customer AddCustomer(Customer customer)
        {
            _context.Customers.Add(customer);
            return customer;
        }

        public int DeleteCustomer(Customer customer)
        {
            _context.Customers.Remove(customer); 
            return 1;
        }

        public IEnumerable<Customer> GetAllCustomers()
        {
            return _context.Customers.ToList();
        }

        public Customer GetCustomer(int id)
        {
            return _context.Customers.FirstOrDefault(c => c.Id == id);
        }

        public IEnumerable<Customer> SearchCustomers(string mailAddress)
        {
            return _context.Customers.Where(c=>c.Email.Contains(mailAddress));
        }

        public Customer UpdateCustomer(Customer customer)
        {
            _context.Customers.Update(customer);
            return customer;
        }
    }
}
