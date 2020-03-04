using System;
using System.Collections.Generic;
using PaymentGateway.Core.Events;
using PaymentGateway.SharedKernel;

namespace PaymentGateway.Core.Model
{
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

        public Guid Id { get; private set; }
        public int Version { get; private set; }

        public decimal Amount { get; private set; }

        public string Currency { get; private set; }

        public PaymentCard Card { get; private set; }

        public IEnumerable<EntityEvent> GetUncommittedChanges()
            => _events;

        public void MarkChangesAsCommitted()
            => _events.Clear();

        private void Apply(PaymentCreated @event)
        {
            Id = @event.Id;
            Amount = @event.Amount;
            Currency = @event.Currency;
            Card = @event.Card;
        }


        private void LoadsFromHistory(IEnumerable<EntityEvent> history)
        {
            foreach (var e in history)
                ApplyChange(e, false);
        }

        private void ApplyChange(EntityEvent @event, bool isNew = true)
        {
            dynamic dynamicThis = this;
            dynamicThis.Apply(@event as dynamic);
            Version = @event.Version;

            if (isNew)
                _events.Add(@event);
        }
    }
}
