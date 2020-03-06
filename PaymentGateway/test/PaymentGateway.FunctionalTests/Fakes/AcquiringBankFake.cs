using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PaymentGateway.Application.BankProviders;
using PaymentGateway.Core.Model;

namespace PaymentGateway.FunctionalTests.Fakes
{
    public class AcquiringBankFake: IAcquiringBank
    {
        public Task<BankPaymentResult> Process(Payment payment)
        {
            return Task.FromResult(new BankPaymentResult()
            {
                Success = true,
                Id = Guid.NewGuid().ToString()
            });
        }
    }
}
