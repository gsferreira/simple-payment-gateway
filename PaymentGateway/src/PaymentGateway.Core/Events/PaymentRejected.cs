using System;
using System.Collections.Generic;
using System.Text;
using PaymentGateway.Core.Model;
using PaymentGateway.SharedKernel;

namespace PaymentGateway.Core.Events
{
    public class PaymentRejected : EntityEvent
    {
        public PaymentRejected(Guid id, string reason)
        {
            Id = id;
            Reason = reason;
        }

        public Guid Id { get; }
        public string Reason { get; }
    }
}
