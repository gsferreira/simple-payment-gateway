using System;
using System.Collections.Generic;
using System.Text;
using PaymentGateway.Core.Model;
using PaymentGateway.SharedKernel;

namespace PaymentGateway.Core.Events
{
    public class PaymentCreated : EntityEvent
    {
        public PaymentCreated(Guid id, decimal amount, string currency, PaymentCard card)
        {
            Id = id;
            Version = 1;
            Amount = amount;
            Currency = currency;
            Card = card;
        }

        public Guid Id { get; }
        public decimal Amount { get; }
        public string Currency { get; }
        public PaymentCard Card { get;  }
    }
}
