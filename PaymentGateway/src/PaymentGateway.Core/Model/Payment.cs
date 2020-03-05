using System;
using System.Collections.Generic;
using PaymentGateway.Core.Events;
using PaymentGateway.SharedKernel;

namespace PaymentGateway.Core.Model
{
    public enum PaymentStates
    {
        Created,
        Processed,
        Rejected
    }
    public class Payment : IEventSourcedEntity
    {
     
        private readonly IList<EntityEvent> _events = new List<EntityEvent>();

        public Payment(IEnumerable<EntityEvent> history)
            => LoadsFromHistory(history);

        public Payment(decimal amount, string currency, PaymentCard paymentCard)
        {
            if (amount <= 0) throw new ArgumentException("Payment amount can't be zero or negative.", nameof(amount));
            if (string.IsNullOrEmpty(currency)) throw new ArgumentNullException(nameof(currency));
            if (paymentCard == null) throw new ArgumentNullException(nameof(paymentCard));
            if (DateTime.Today > new DateTime(paymentCard.ExpireYear, paymentCard.ExpireMonth, DateTime.Today.Day))
                throw new ArgumentException("Payment card expired.");

            ApplyChange(new PaymentCreated(Guid.NewGuid(),
                amount,
                currency,
                paymentCard));
        }

        public PaymentStates State { get; private set; }
        public Guid Id { get; private set; }
        public int Version { get; private set; }

        public decimal Amount { get; private set; }

        public string Currency { get; private set; }

        public PaymentCard Card { get; private set; }

        public string BankId { get; private set; }
        public string RejectionReason { get; private set; }

        public void Processed(string bankId)
        {
            if (string.IsNullOrEmpty(bankId)) throw new ArgumentNullException(nameof(bankId));

            ApplyChange(new PaymentProcessed(Id,
                bankId));
        }

        public void Rejected(string reason)
        {
            ApplyChange(new PaymentRejected(Id,
                reason));
        }

        public IEnumerable<EntityEvent> GetUncommittedChanges()
            => _events;

        public void MarkChangesAsCommitted()
            => _events.Clear();

        private void Apply(PaymentCreated @event)
        {
            State = PaymentStates.Created;
            Id = @event.Id;
            Amount = @event.Amount;
            Currency = @event.Currency;
            Card = @event.Card;
        }

        private void Apply(PaymentProcessed @event)
        {
            State = PaymentStates.Processed;
            BankId = @event.BankId;
        }

        private void Apply(PaymentRejected @event)
        {
            State = PaymentStates.Rejected;
            RejectionReason = @event.Reason;
        }


        private void LoadsFromHistory(IEnumerable<EntityEvent> history)
        {
            foreach (var e in history)
                ApplyChange(e, false);
        }

        private void ApplyChange(EntityEvent @event, bool isNew = true)
        {
            if (isNew)
                @event.Version = Version + 1;

            dynamic dynamicThis = this;
            dynamicThis.Apply(@event as dynamic);
            Version = @event.Version;

            if (isNew)
                _events.Add(@event);
        }
    }
}
