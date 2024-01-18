using CustomersOrders.Classes;
using CustomersOrders.Handlers.Customers.Commands;
using CustomersOrders.Handlers.Customers.Queries;
using CustomersOrders.Handlers.Orders.Commands;
using CustomersOrders.Handlers.Orders.Queries;
using CustomersOrders.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CustomersOrders.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {

        private readonly ILogger<CustomerController> _logger;
        private readonly IMediator _mediator;

        public CustomerController(IMediator mediator, ILogger<CustomerController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("All")]
        public async Task<IActionResult> GetAll()
        {

            var result =await _mediator.Send(new GetAllCustomersQuery());
            return Ok(result);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            
            var result =await _mediator.Send(new GetCustomerQuery() {Id=id });
            return Ok(result);
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CustomerAdd create)
        {
            var addCustomer = new CreateCustomerCommand() { CustomerAdd = create};
            var result =await _mediator.Send(addCustomer);
            return Ok(result);
        }
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] UpdateCustomerCommand update)
        {
            var result = await _mediator.Send(update);

            return Ok(result); 
        }
    
        [HttpDelete("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result =await _mediator.Send(new DeleteCustomerCommand() { Id=id });
            return Ok(result);
        }
        [HttpGet("Search/{mail}")]
        public async Task<IActionResult> Search(string mail)
        {
            var result =await _mediator.Send(new SearchCustomersQuery() { MailAddress=mail });
            return Ok(result);
        }

        [HttpPost("CreateOrder")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderCommand create)
        {
            var result =await _mediator.Send(create);
            return Ok(result);
        }
    }
}
