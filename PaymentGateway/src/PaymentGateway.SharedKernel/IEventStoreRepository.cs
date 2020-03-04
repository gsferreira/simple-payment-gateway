using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.SharedKernel
{
    public interface IEventStoreRepository<T>
        where T : IEventSourcedEntity
    {
        Task<T> Get(Guid aggregateId);

        Task Save(Guid aggregateId, T data, int? expectedVersion = null);
    }
}
