using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Core.Commands;

namespace PaymentGateway.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PaymentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PaymentCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result)
                return BadRequest();

            return Created($"api/payments/", null); //TODO: Return created status code
        }
    }
}