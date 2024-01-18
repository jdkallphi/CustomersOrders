using AutoMapper;
using Azure.Core;
using Castle.Core.Resource;
using CustomersOrders.Classes;
using CustomersOrders.Handlers.Customers;
using CustomersOrders.Handlers.Customers.Commands;
using CustomersOrders.Handlers.Customers.Queries;
using CustomersOrders.Models;
using CustomersOrders.Models.DTO;
using CustomersOrders.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using Moq;
using NUnit.Framework;

namespace CustomersOrders.UnitTesting
{

    [TestFixture]
    public class CustomerTest
    {
        Mock<ICustomerRepository>  _mockRepository = new Mock<ICustomerRepository>();
        Mock<IMapper> _mockMapper = new Mock<IMapper>();
        Mock<IUnitOfWork> _unitOfWork = new Mock<IUnitOfWork>();
        [SetUp]
        public void Setup()
        {
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
            _mockMapper.Setup(x => x.Map<CustomerDTO>(It.IsAny<CustomerDTO>()))
                .Returns(customerDTO);
            _mockMapper.Setup(x => x.Map<Customer>(It.IsAny<CustomerDTO>()))
                .Returns(customer);
            _mockRepository.Setup(x => x.AddCustomer(It.IsAny<Customer>()))
            .Returns(customer);
            _mockRepository.Setup(x => x.GetCustomer(It.IsAny<int>()))
            .Returns(customer);
        }

        [Test]
        public async Task Should_Create_A_Customer()
        {
            // Arrange
            var customer = await CreateNewCustomer(_mockRepository, _mockMapper, _unitOfWork);

            // Assert
            Assert.That(customer, Is.Not.Null);
            Assert.That(customer, Is.InstanceOf<CustomerDTO>());

            _mockRepository.Verify(repo => repo.AddCustomer(It.IsAny<Customer>()), Times.Once);

            var handler = new GetCustomerHandler(_mockRepository.Object, _mockMapper.Object);
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
            var handler = new GetAllCustomersHandler(_mockRepository.Object, _mockMapper.Object);
            var command = new GetAllCustomersQuery
            {};
            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<CustomerDTO[]>());
        }

        //should get customer by id
        [Test]
        public async Task Should_Get_Customer_By_Id()
        {

            // Arrange
            var customer = await CreateNewCustomer(_mockRepository, _mockMapper, _unitOfWork);
            var existingCustomers = new List<Customer> { };
            var mockValidator = new Mock<CustomerValidator>(existingCustomers);
            mockValidator.CallBase = true;
            var handler = new GetCustomerHandler(_mockRepository.Object, _mockMapper.Object);
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
            var customer =await CreateNewCustomer(_mockRepository, _mockMapper, _unitOfWork);

            // Assert
            Assert.That(customer, Is.Not.Null);
            Assert.That(customer, Is.InstanceOf<CustomerDTO>());

            var handlerUpdate = new UpdateCustomerHandler(_mockRepository.Object, _mockMapper.Object, _unitOfWork.Object);
            var commandUpdate = new UpdateCustomerCommand
            {

                Email = "",
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

            var handler = new GetCustomerHandler(_mockRepository.Object, _mockMapper.Object);
            var command = new GetCustomerQuery
            { Id = customer.Id };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<CustomerDTO>());

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<CustomerDTO>());
        }

        private async Task<CustomerDTO> CreateNewCustomer(Mock<ICustomerRepository> repo, Mock<IMapper> map, Mock<IUnitOfWork> _unitOfWork)
        {

            var existingCustomers = new List<Customer> { };
            var mockValidator = new Mock<CustomerValidator>(existingCustomers);
            mockValidator.CallBase = true;

            var handler = new CreateCustomerHandler(repo.Object, map.Object, _unitOfWork.Object);

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

            map.Setup(m => m.Map<Customer>(It.IsAny<CustomerAdd>())).Returns(customer);
            map.Setup(m => m.Map<CustomerDTO>(It.IsAny<Customer>())).Returns(customerDTO);

            // Act
            return await handler.Handle(command, CancellationToken.None);
        }
    }
}
