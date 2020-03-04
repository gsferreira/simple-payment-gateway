using System.Collections.Generic;
using System.Threading.Tasks;

namespace PaymentGateway.SharedKernel
{
    public interface IDeferredEventDispatcher
    {
        Task Dispatch(IEnumerable<EntityEvent> events);
    }
}