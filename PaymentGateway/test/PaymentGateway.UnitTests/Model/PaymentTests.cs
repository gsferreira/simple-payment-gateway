using System;
using System.Linq;
using PaymentGateway.Core.Events;
using Xunit;
using PaymentGateway.Core.Model;

namespace PaymentGateway.UnitTests.Model
{
    public class PaymentTests
    {
        [Fact]
        public void Create_WithValidData_Success()
        {
            var payment = new Payment((decimal) 123.56, "EUR",
                new PaymentCard("VISA", "Tyrion Lannister", 4532367296473418,
                    12, DateTime.Today.Year, 765));

            Assert.Single(payment.GetUncommittedChanges());
            Assert.IsType<PaymentCreated>(payment.GetUncommittedChanges().First());

        }
    }
}