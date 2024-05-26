using Microsoft.AspNetCore.Mvc;
using Order.API.Commands.CreateOrder;
using Shared.Interfaces.Commands;

namespace Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;

        public OrdersController(ICommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateOrderCommand createOrder, CancellationToken cancellationToken)
        {
            var result = await _commandDispatcher.Dispatch<CreateOrderCommand, CreateOrderCommandResult>(createOrder, cancellationToken);

            if (!result.Status)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
