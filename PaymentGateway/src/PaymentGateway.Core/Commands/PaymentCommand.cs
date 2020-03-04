using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using PaymentGateway.Core.Model;
using PaymentGateway.SharedKernel;

namespace PaymentGateway.Core.Commands
{
    public class PaymentCommand : IRequest<Payment>
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public PaymentCardCommand Card { get; set; }

        public class PaymentCardCommand
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
