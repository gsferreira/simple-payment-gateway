using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PaymentGateway.SharedKernel;

namespace PaymentGateway.Infrastructure.Events
{
    public class DeferredEventDispatcher: IDeferredEventDispatcher
    {
        private readonly IMediator _mediator;

        public DeferredEventDispatcher(IMediator mediator)
            => _mediator = mediator;

        public Task Dispatch(IEnumerable<EntityEvent> events)
            => Task.WhenAll(events.Select(ev => _mediator.Publish(ev)));
    }
}
