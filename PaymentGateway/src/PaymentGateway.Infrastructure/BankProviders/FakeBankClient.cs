using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PaymentGateway.Application.BankProviders;
using PaymentGateway.Core.Model;

namespace PaymentGateway.Infrastructure.BankProviders
{
    public class FakeBankClient : IAcquiringBank
    {
        public Task<BankPaymentResult> Process(Payment payment)
        {
            throw new NotImplementedException();
        }
    }
}
