using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Api.Application.Commands;
using PaymentGateway.Api.Application.Queries;
using PaymentGateway.Api.Models;

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

            if (result == null)
                return BadRequest();

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