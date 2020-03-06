using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Application.Commands;
using PaymentGateway.Application.Queries;
using PaymentGateway.Core.Model;

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
            var result = await _mediator.Send<Payment>(command);

            if (result == null)
                return BadRequest();

            if (result.State == PaymentStates.Rejected)
                return BadRequest(result.RejectionReason);

            return Created($"api/payments/{result.Id}", PaymentDto.FromPayment(result));
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send<PaymentQuery.Result>(new PaymentQuery(id));

            if (result?.Data == null)
                return NotFound();

            return Ok(result.Data);
        }

    }
}