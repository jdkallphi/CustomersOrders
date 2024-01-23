using CustomersOrders.Models.Customers;
using FluentValidation;
using System.Numerics;

namespace CustomersOrders.Classes
{
    public class CustomerValidator: AbstractValidator<Customer>
    {
        private readonly IEnumerable<Customer> _customers;
        public CustomerValidator(IEnumerable<Customer> customers)
        {
            _customers = customers;
            RuleFor(customer => customer.FirstName).NotEmpty().WithMessage("First name is required");
            RuleFor(customer => customer.LastName).NotEmpty().WithMessage("Last name is required");
            RuleFor(customer => customer.Email).NotEmpty().WithMessage("Email is required");
            RuleFor(customer => customer.Email).EmailAddress().WithMessage("Email is not correct format");
            RuleFor(customer => customer.Email).Must(IsEmailUnique).WithMessage("Email is not unique");

        }

        private bool IsEmailUnique(Customer editedCustomer, string newEmail)
        {
            return !_customers.Any(customer => customer.Id != editedCustomer.Id && customer.Email == newEmail);
        }
    }
}
