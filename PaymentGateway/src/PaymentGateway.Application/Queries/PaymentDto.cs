using System;
using PaymentGateway.Core.Model;

namespace PaymentGateway.Application.Queries
{
    public class PaymentDto
    {
        public string State { get; set; }
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public PaymentCardDto Card { get; set; }
        public string BankId { get; set; }
        public string RejectionReason { get; set; }
        public static PaymentDto FromPayment(Payment payment)
        {
            return new PaymentDto()
            {
                State = payment.State.ToString(),
                Id = payment.Id,
                Amount = payment.Amount,
                Currency = payment.Currency,
                BankId = payment.BankId,
                RejectionReason = payment.RejectionReason,
                Card = new PaymentCardDto()
                {
                    CVV = payment.Card.CVV,
                    ExpireMonth = payment.Card.ExpireMonth,
                    ExpireYear= payment.Card.ExpireYear,
                    Name= payment.Card.Name,
                    Number = $"{payment.Card.Number.ToString().Substring(0, 4)}##########",
                    Type= payment.Card.Type,
                }
            };
        }

        public class PaymentCardDto
        {
            public string Type { get; set; }
            public string Name { get; set; }
            public string Number { get; set; }
            public int ExpireMonth { get; set; }
            public int ExpireYear { get; set; }
            public int CVV { get; set; }
        }
    }
}
