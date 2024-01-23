using CustomersOrders.Models.Customers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomersOrders.Models.Orders
{
    public class Order
    {
        public Order() { }
        [Key]
        public int Id { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsCancelled { get; set; }
        public virtual int CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }

        public Order SetDescription(string description)
        {
            Description = description;
            return this;
        }
        public Order SetPrice(decimal price)
        {
            Price = price;
            return this;
        }
        public Order SetCreationDate()
        {
            CreationDate = DateTime.UtcNow;
            return this;
        }
        public Order SetIsCancelled()
        {
            IsCancelled = true;
            return this;
        }
        public Order SetIsNotCancelled()
        {
            IsCancelled = false;
            return this;
        }

        public Order SetCustomer(Customer customer)
        {
            Customer = customer;
            return this;
        }


    }
}
