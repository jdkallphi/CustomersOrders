using CustomersOrders.Classes;
using FluentValidation;

namespace CustomersOrders
{
    public class OrderValidator: AbstractValidator<Order>
    {
       public OrderValidator() { 
            RuleFor(order => order.CreationDate).NotEmpty().WithMessage("Creation date is required");
            RuleFor(order => order.Price).NotEmpty().WithMessage("Price is required");
            RuleFor(order => order.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
            RuleFor(order => order.Description).MaximumLength(100).WithMessage("Description must be less than 100 characters");
            RuleFor(order => order.Customer).NotEmpty().WithMessage("Customer is required");
        }
    }
}
