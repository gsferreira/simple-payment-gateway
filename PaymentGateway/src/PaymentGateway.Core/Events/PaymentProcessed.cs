using System;
using System.Collections.Generic;
using System.Text;
using PaymentGateway.Core.Model;
using PaymentGateway.SharedKernel;

namespace PaymentGateway.Core.Events
{
    public class PaymentProcessed : EntityEvent
    {
        public PaymentProcessed(Guid id, string bankId)
        {
            Id = id;
            BankId = bankId;
        }

        public Guid Id { get; }
        public string BankId { get; }
    }
}
