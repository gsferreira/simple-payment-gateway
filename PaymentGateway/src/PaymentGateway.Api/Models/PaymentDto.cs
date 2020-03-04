using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PaymentGateway.Core.Model;

namespace PaymentGateway.Api.Models
{
    public class PaymentDto
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public PaymentCardDto Card { get; set; }

        public static PaymentDto FromPayment(Payment payment)
        {
            return new PaymentDto()
            {
                Id = payment.Id,
                Amount = payment.Amount,
                Currency = payment.Currency,
                Card = new PaymentCardDto()
                {
                    CVV = payment.Card.CVV,
                    ExpireMonth = payment.Card.ExpireMonth,
                    ExpireYear= payment.Card.ExpireYear,
                    Name= payment.Card.Name,
                    Number = payment.Card.Number,
                    Type= payment.Card.Type,
                }
            };
        }

        public class PaymentCardDto
        {
            public string Type { get; set; }
            public string Name { get; set; }
            public long Number { get; set; }
            public int ExpireMonth { get; set; }
            public int ExpireYear { get; set; }
            public int CVV { get; set; }
        }
    }
}
