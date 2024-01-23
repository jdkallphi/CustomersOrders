using AutoMapper;
using CustomersOrders.Classes;
using CustomersOrders.Handlers.Customers;
using CustomersOrders.Handlers.Customers.Commands;
using CustomersOrders.Handlers.Customers.Queries;
using CustomersOrders.Handlers.Orders;
using CustomersOrders.Handlers.Orders.Commands;
using CustomersOrders.Handlers.Orders.Queries;
using CustomersOrders.Models;
using CustomersOrders.Models.Customers;
using CustomersOrders.Models.Orders;
using CustomersOrders.Repositories;
using Moq;
using System.Net.Mail;
using NUnit.Framework;

namespace CustomersOrders.UnitTesting
{

    [TestFixture]
    public class OrderTest
    {
        Mock<ICustomerRepository> _mockCustomerRepository = new Mock<ICustomerRepository>();
        Mock<IOrderRepository>  _mockOrderRepository = new Mock<IOrderRepository>();
        Mock<IUnitOfWork> _unitOfWork = new Mock<IUnitOfWork>();
        Mock<MailMessage> _mockMailMessage = new Mock<MailMessage>();
        private static IMapper _mapper;

        [SetUp]
        public void Setup()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapperProfile());
            });
            mappingConfig.AssertConfigurationIsValid();

            IMapper mapper = mappingConfig.CreateMapper();
            _mapper = mapper;

            var customerDTO = new CustomerDTO()
            {
                Email = "",
                FirstName = "John",
                LastName = "Doe"
            };
            var customer = new Customer()
            {
                Id = 1,
                Email = "johnDoe@test.be",
                FirstName = "John",
                LastName = "Doe",
                NumberOfOrders = 0,
            };
            var orderDTO = new OrderDTO()
            {
                Id = 1,
                Description="desc",
                Price = 155.55M
            };
            var order = new Order()
            {
                Id = 1,
                CustomerId = 1,
                Description = "new order",
                Price = 155.55M,
                CreationDate = DateTime.Now,
                IsCancelled= false
            };
            var cancelledOrders = new List<Order>()
            {
                new Order()
                {
                    Id = 1,
                    CustomerId = 1,
                    Description = "cancelled order",
                    Price = 123.45M,
                    CreationDate = DateTime.Now,
                    IsCancelled= true
                }
            };
          
            _mockCustomerRepository.Setup(x=>x.GetAllCustomers())
                .Returns(new List<Customer>() { customer });
            _mockOrderRepository.Setup(x => x.GetAllCancelledOrders(It.IsAny<int>()))
                .Returns(cancelledOrders);
            _mockOrderRepository.Setup(x => x.SearchOrders(It.IsAny<IEnumerable<int>>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns([order]);
            _mockOrderRepository.Setup(x => x.GetOrderById(It.IsAny<int>()))
                .Returns(order);
            _mockCustomerRepository.Setup(x => x.AddCustomer(It.IsAny<Customer>()))
                .Returns(customer);
            _mockCustomerRepository.Setup(x => x.GetCustomer(It.IsAny<int>()))
                .Returns(customer);
        }

        [Test]
        public async Task Should_Add_Order_To_Customer()
        {
            // Arrange
            var customer = await CreateNewCustomer(_mockCustomerRepository , _mapper, _unitOfWork);
            // Assert
            Assert.That(customer, Is.Not.Null);
            Assert.That(customer, Is.InstanceOf<CustomerDTO>());

            _mockCustomerRepository.Verify(repo => repo.AddCustomer(It.IsAny<Customer>()), Times.Once);

            var custHandler = new GetCustomerQueryHandler(_mockCustomerRepository.Object, _mapper);
            var command = new GetCustomerQuery
            { Id= customer.Id};
           
            // Act
            var custResult = await custHandler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(custResult, Is.Not.Null);
            Assert.That(custResult, Is.InstanceOf<CustomerDTO>());

            var addOrderhandler = new CreateOrderCommandHandler(_mockCustomerRepository.Object, _mockOrderRepository.Object, _mapper, _unitOfWork.Object, _mockMailMessage.Object);

            var orderCommand = new CreateOrderCommand
            {
                CustomerId = customer.Id,
                Order = new OrderAdd
                {
                    Description = "new order",
                    Price = 155.55M
                }
            };

            // Act
            var orderResult = await addOrderhandler.Handle(orderCommand, CancellationToken.None);
            
            var result = _mockCustomerRepository.Object.GetCustomer(customer.Id);

            Assert.That(result.Orders, Is.Not.Null);
            Assert.That(result.Orders.Count, Is.AtLeast(1));

        }

        [Test]
        public async Task Should_Get_All_Cancelled_Orders()
        {
            // Arrange
            var existingCustomers = new List<Customer> { };
            var mockValidator = new Mock<CustomerValidator>(existingCustomers);
            mockValidator.CallBase = true;
            var handler = new GetAllCustomersQueryHandler(_mockCustomerRepository .Object, _mapper);
            var command = new GetAllCustomersQuery
            {};
            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<IEnumerable<CustomerDTO>>());

            var cancelledHandlers = new GetCancelledOrdersQueryHandler(_mapper, _mockOrderRepository.Object, _mockCustomerRepository.Object);
            var cancelledCommand = new GetCancelledOrdersQuery
            { 
                CustomerId= result.First().Id
            };
            // Act
            var cancelledResult = await cancelledHandlers.Handle(cancelledCommand, CancellationToken.None);

            // Assert

            Assert.That(cancelledResult, Is.Not.Null);
            Assert.That(cancelledResult, Is.InstanceOf<IEnumerable<OrderDTO>>());
            Assert.That(cancelledResult.Count, Is.AtLeast(1));
        }

        [Test]
        public async Task Should_Search_Orders()
        {

            // Arrange
            var customer = await CreateNewCustomer(_mockCustomerRepository , _mapper, _unitOfWork);
            var existingCustomers = new List<Customer> { };
            var mockValidator = new Mock<CustomerValidator>(existingCustomers);
            mockValidator.CallBase = true;
            var handler = new GetCustomerQueryHandler(_mockCustomerRepository .Object, _mapper);
            var command = new GetCustomerQuery
            {
                Id = 1
            };
            // Act
            var result = await handler.Handle(command, CancellationToken.None);
            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<CustomerDTO>());

            var searchHandler = new SearchOrdersQueryHandler(_mapper, _mockOrderRepository.Object);
            var searchCommand = new SearchOrdersQuery
            {
                CustomerIds = new List<int> { customer.Id },
                StartDate = DateTime.Now.AddDays(-1),
                EndDate = DateTime.Now.AddDays(1)
            };
            // Act
            var searchResult = await searchHandler.Handle(searchCommand, CancellationToken.None);

            // Assert

            Assert.That(searchResult, Is.Not.Null);
            Assert.That(searchResult, Is.InstanceOf<IEnumerable<OrderDTO>>());
            Assert.That(searchResult.Count, Is.AtLeast(1));
        }
       
        private async Task<CustomerDTO> CreateNewCustomer(Mock<ICustomerRepository> repo, IMapper map, Mock<IUnitOfWork> _unitOfWork)
        {
            var existingCustomers = new List<Customer> { };
            var mockValidator = new Mock<CustomerValidator>(existingCustomers);
            mockValidator.CallBase = true;

            var handler = new CreateCustomerCommandHandler(repo.Object, map, _unitOfWork.Object);

            var command = new CreateCustomerCommand
            {
                CustomerAdd = new CustomerAdd
                {
                    Email = "john@doe.be",
                    FirstName = "John",
                    LastName = "Doe"
                }
            };

            // Act
            return await handler.Handle(command, CancellationToken.None);
        }
    }
}
