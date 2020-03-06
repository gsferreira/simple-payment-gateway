using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PaymentGateway.Application.BankProviders;
using PaymentGateway.Core.Events;

namespace PaymentGateway.Application.EventHandlers
{
    public class PaymentCreatedEventHandler: INotificationHandler<PaymentCreated>
    {
        private readonly IAcquiringBank _acquiringBank;

        public PaymentCreatedEventHandler(IAcquiringBank acquiringBank)
        {
            _acquiringBank = acquiringBank;
        }

        public Task Handle(PaymentCreated notification, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
