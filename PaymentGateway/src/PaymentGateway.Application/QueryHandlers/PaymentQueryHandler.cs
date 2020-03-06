using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PaymentGateway.Application.Queries;
using PaymentGateway.Core.Model;
using PaymentGateway.SharedKernel;

namespace PaymentGateway.Application.QueryHandlers
{
    public class PaymentQueryHandler :
        IRequestHandler<PaymentQuery, PaymentQuery.Result>
    {
        private readonly IEventStoreRepository<Payment> _eventStore;

        public PaymentQueryHandler(IEventStoreRepository<Payment> eventStore)
        {
            _eventStore = eventStore;
        }

        public async Task<PaymentQuery.Result> Handle(PaymentQuery request, CancellationToken cancellationToken)
        {
            var payment = await _eventStore.Get(request.Id);
            return payment != null ? new PaymentQuery.Result(payment) : null;
        }
    }
}
