using AutoMapper;
using CustomersOrders.Classes;
using CustomersOrders.Handlers.Customers.Queries;
using CustomersOrders.Handlers.Orders.Commands;
using CustomersOrders.Repositories;
using FluentValidation;
using MediatR;
using System.Net;
using System.Net.Mail;

namespace CustomersOrders.Handlers.Orders
{
    public class CreateOrderHandler: IRequestHandler<CreateOrderCommand, int>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly MailMessage _mailMessage;
        private readonly IUnitOfWork _unitOfWork; 
        
        public CreateOrderHandler(ICustomerRepository customerRepository, IOrderRepository orderRepository, IMapper mapper, IUnitOfWork unitOfWork, MailMessage mail)
        {
            _mapper = mapper;
            _mailMessage = mail;
            _customerRepository = customerRepository;
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
        }
        public Task<int> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = _mapper.Map<Order>(request.Order);
            order.SetCreationDate();
            var customer = _customerRepository.GetCustomer(request.CustomerId);
            if (customer == null)
            {
                throw new Exception("customer not found");
            }
            customer.AddOrder(order);
            var validator = new OrderValidator();
            validator.ValidateAndThrow(order);
            _unitOfWork.SaveChanges();

            using (var client = new SmtpClient("smtp.gmail.com"))
            {
                client.Port = 587; // Specify the SMTP port
                client.Credentials = new NetworkCredential("gmailUsername", "gmailPassword");
                client.EnableSsl = true; // Enable SSL if required

                _mailMessage.From = new MailAddress("Allphi@allphi.eu");
                _mailMessage.To.Add(customer.Email);

                _mailMessage.Subject = "new order";
                _mailMessage.Body = "thanks for spending money";
                _mailMessage.IsBodyHtml = true; 
                _mailMessage.Priority = MailPriority.Normal; 

                _mailMessage.CC.Add("accounting@allphi.com");

                //client.Send(_mailMessage);
            }

            return Task.FromResult(order.Id);
        }
    }
}
