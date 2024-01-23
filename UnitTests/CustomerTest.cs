using AutoMapper;
using CustomersOrders.Classes;
using CustomersOrders.Handlers.Customers;
using CustomersOrders.Handlers.Customers.Commands;
using CustomersOrders.Handlers.Customers.Queries;
using CustomersOrders.Models;
using CustomersOrders.Models.Customers;
using CustomersOrders.Repositories;
using Moq;
using NUnit.Framework;

namespace CustomersOrders.UnitTesting
{

    [TestFixture]
    public class CustomerTest
    {
        Mock<ICustomerRepository>  _mockRepository = new Mock<ICustomerRepository>();
        Mock<IUnitOfWork> _unitOfWork = new Mock<IUnitOfWork>();
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
                Email = "johnDoe@test.be",
                FirstName = "John",
                LastName = "Doe",
                Id = 1,
                NumberOfOrders = 0
            };
            var customer = new Customer()
            {
                Id = 1,
                Email = "johnDoe@test.be",
                FirstName = "John",
                LastName = "Doe",
                NumberOfOrders = 0,
            };

            _mockRepository.Setup(x => x.AddCustomer(It.IsAny<Customer>()))
            .Returns(customer);
            _mockRepository.Setup(x => x.GetCustomer(It.IsAny<int>()))
            .Returns(customer);
            _mockRepository.Setup(x => x.GetAllCustomers())
                .Returns(new List<Customer>() { customer }.AsQueryable());
        }

        [Test]
        public async Task Should_Create_A_Customer()
        {
            // Arrange
            var customer = await CreateNewCustomer(_mockRepository, _mapper, _unitOfWork);

            // Assert
            Assert.That(customer, Is.Not.Null);
            Assert.That(customer, Is.InstanceOf<CustomerDTO>());

            _mockRepository.Verify(repo => repo.AddCustomer(It.IsAny<Customer>()), Times.Once);

            var handler = new GetCustomerQueryHandler(_mockRepository.Object, _mapper);
            var command = new GetCustomerQuery
            { Id= customer.Id};
           
            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<CustomerDTO>());
        }

      

        //should get all customers
        [Test]
        public async Task Should_Get_All_Customers()
        {
            // Arrange
            var existingCustomers = new List<Customer> { };
            var mockValidator = new Mock<CustomerValidator>(existingCustomers);
            mockValidator.CallBase = true;
            var handler = new GetAllCustomersQueryHandler(_mockRepository.Object, _mapper);
            var command = new GetAllCustomersQuery();
            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<IEnumerable<CustomerDTO>> ());
            Assert.That(result.Count, Is.AtLeast(1));
        }

        //should get customer by id
        [Test]
        public async Task Should_Get_Customer_By_Id()
        {

            // Arrange
            var customer = await CreateNewCustomer(_mockRepository, _mapper, _unitOfWork);
            var existingCustomers = new List<Customer> { };
            var mockValidator = new Mock<CustomerValidator>(existingCustomers);
            mockValidator.CallBase = true;
            var handler = new GetCustomerQueryHandler(_mockRepository.Object, _mapper);
            var command = new GetCustomerQuery
            {
                Id = 1
            };
            // Act
            var result = await handler.Handle(command, CancellationToken.None);
            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<CustomerDTO>());

        }

        //should update customer
        [Test]
        public async Task Should_Update_Customer()
        {

            // Arrange
            var customer =await CreateNewCustomer(_mockRepository, _mapper, _unitOfWork);

            // Assert
            Assert.That(customer, Is.Not.Null);
            Assert.That(customer, Is.InstanceOf<CustomerDTO>());

            var handlerUpdate = new UpdateCustomerCommandHandler(_mockRepository.Object, _mapper, _unitOfWork.Object);
            var commandUpdate = new UpdateCustomerCommand
            {

                Email = "doedoe@johnjohn.be",
                FirstName = "John",
                LastName = "Doe",
                Id = customer.Id

            };
            // Act
            var resultUpdate = await handlerUpdate.Handle(commandUpdate, CancellationToken.None);

            var customer1 = new Customer()
            {
                Id = 1,
                Email = "johnDoe@test.be",
                FirstName = "John",
                LastName = "Doe",
                NumberOfOrders = 0,
            };

            var handler = new GetCustomerQueryHandler(_mockRepository.Object, _mapper);
            var command = new GetCustomerQuery
            { Id = customer.Id };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<CustomerDTO>());

        }
        //should update customer
        [Test]
        public async Task Should_Not_Update_Customer_With_Empty_Email()
        {

            // Arrange
            var customer = await CreateNewCustomer(_mockRepository, _mapper, _unitOfWork);

            // Assert
            Assert.That(customer, Is.Not.Null);
            Assert.That(customer, Is.InstanceOf<CustomerDTO>());

            var handlerUpdate = new UpdateCustomerCommandHandler(_mockRepository.Object, _mapper, _unitOfWork.Object);
            var commandUpdate = new UpdateCustomerCommand
            {

                Email = "",
                FirstName = "John",
                LastName = "Doe",
                Id = customer.Id

            };
            // Act
            Assert.That(async () => await handlerUpdate.Handle(commandUpdate, CancellationToken.None), Throws.TypeOf<FluentValidation.ValidationException>().And.Message.Contain("Email"));
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
            var customer = new Customer
            {
                Email = "john@doe.be",
                FirstName = "John",
                LastName = "Doe"
            };
            var customerDTO = new CustomerDTO
            {
                Email = "john@doe.be",
                FirstName = "John",
                LastName = "Doe"
            };

           // map.Setup(m => m.Map<Customer>(It.IsAny<CustomerAdd>())).Returns(customer);
            //map.Setup(m => m.Map<CustomerDTO>(It.IsAny<Customer>())).Returns(customerDTO);

            // Act
            return await handler.Handle(command, CancellationToken.None);
        }
    }
}
