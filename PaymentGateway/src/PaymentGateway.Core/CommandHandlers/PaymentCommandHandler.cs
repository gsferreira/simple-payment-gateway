using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PaymentGateway.Core.Commands;
using PaymentGateway.Core.Model;
using PaymentGateway.SharedKernel;

namespace PaymentGateway.Core.CommandHandlers
{
    public class PaymentCommandHandler : IRequestHandler<PaymentCommand, bool>
    {
        private readonly IEventStoreRepository<Payment> _eventStore;

        public PaymentCommandHandler(IEventStoreRepository<Payment> eventStore)
        {
            _eventStore = eventStore;
        }

        public async Task<bool> Handle(PaymentCommand request, CancellationToken cancellationToken)
        {
            var payment = new Payment(request.Amount, request.Currency,
                new PaymentCard(request.Card.Type, request.Card.Name, request.Card.Number, request.Card.ExpireMonth,
                    request.Card.ExpireYear, request.Card.CVV));

            await _eventStore.Save(payment.Id, payment);

            return true;
        }
    }
}