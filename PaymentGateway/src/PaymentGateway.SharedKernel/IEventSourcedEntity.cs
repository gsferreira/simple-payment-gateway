using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGateway.SharedKernel
{
    public interface IEventSourcedEntity
    {
        Guid Id { get; }

        int Version { get; }

        IEnumerable<EntityEvent> GetUncommittedChanges();

        void MarkChangesAsCommitted();

    }
}
