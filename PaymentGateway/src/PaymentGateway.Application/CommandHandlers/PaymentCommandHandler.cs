using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PaymentGateway.Application.BankProviders;
using PaymentGateway.Application.Commands;
using PaymentGateway.Core.Model;
using PaymentGateway.SharedKernel;

namespace PaymentGateway.Application.CommandHandlers
{
    public class PaymentCommandHandler : IRequestHandler<PaymentCommand, Payment>
    {
        private readonly IEventStoreRepository<Payment> _eventStore;
        private readonly IAcquiringBank _acquiringBank;

        public PaymentCommandHandler(IEventStoreRepository<Payment> eventStore,
            IAcquiringBank acquiringBank)
        {
            _eventStore = eventStore;
            _acquiringBank = acquiringBank;
        }

        public async Task<Payment> Handle(PaymentCommand request, CancellationToken cancellationToken)
        {
            var payment = new Payment(request.Amount, request.Currency,
                new PaymentCard(request.Card.Type, request.Card.Name, request.Card.Number, request.Card.ExpireMonth,
                    request.Card.ExpireYear, request.Card.CVV));

            await _eventStore.Save(payment);

            var bankProcessResult = await _acquiringBank.Process(payment);

            if (bankProcessResult.Success)
                payment.Processed(bankProcessResult.Id);
            else
                payment.Rejected(bankProcessResult.Message);

            await _eventStore.Save(payment);

            return payment;
        }
    }
}