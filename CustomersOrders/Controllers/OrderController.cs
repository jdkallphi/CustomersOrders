using CustomersOrders.Handlers.Orders.Commands;
using CustomersOrders.Handlers.Orders.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CustomersOrders.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator, ILogger<CustomerController> logger)
        {
            _mediator = mediator;

            _logger = logger;
        }

        [HttpGet("All/{customerId:int}")]
        public async Task<IActionResult> GetAll(int customerId)
        {
            var result = await _mediator.Send(new GetAllOrdersQuery() { CustomerId = customerId });
            return Ok(result);
        }

        [HttpPost("Search")]
        public async Task<IActionResult> Search([FromBody] SearchOrdersQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpDelete("Cancel/{orderId:int}")]
        public async Task<IActionResult> Cancel(int orderId)
        {
            var result = await _mediator.Send(new CancelOrderCommand() { OrderId = orderId });
            return Ok(result);
        }

        [HttpGet("Cancelled/{customerId:int}")]
        public async Task<IActionResult> Cancelled(int customerId)
        {
            var result = await _mediator.Send(new GetCancelledOrdersQuery() { CustomerId = customerId });
            return Ok(result);
        }
    }
}
