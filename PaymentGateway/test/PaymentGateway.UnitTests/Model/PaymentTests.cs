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
            var payment = new Payment((decimal)123.56, "EUR",
                new PaymentCard("VISA", "Tyrion Lannister", "4532367296473418",
                    12, DateTime.Today.Year, 765));

            Assert.Single(payment.GetUncommittedChanges());
            Assert.IsType<PaymentCreated>(payment.GetUncommittedChanges().First());
        }

        [Fact]
        public void Create_WithAmountZero_FailsWithException()
        {
            Assert.Throws<ArgumentException>(() => new Payment(0, "EUR",
                new PaymentCard("VISA", "Tyrion Lannister", "4532367296473418",
                    12, DateTime.Today.Year, 765))
            );
        }

        [Fact]
        public void Create_WithoutCurrency_FailsWithException()
        {
            Assert.Throws<ArgumentNullException>(() => new Payment(2, null,
                new PaymentCard("VISA", "Tyrion Lannister", "4532367296473418",
                    12, DateTime.Today.Year, 765))
            );
        }

        [Fact]
        public void Create_WithoutCard_FailsWithException()
        {
            Assert.Throws<ArgumentNullException>(() => new Payment(2, null,
                null)
            );
        }

        [Fact]
        public void Create_WithExpiredCard_FailsWithException()
        {
            var expiredDate = DateTime.Today.AddMonths(-1);

            Assert.Throws<ArgumentException>(() =>
                new Payment((decimal)123.56, "EUR",
                    new PaymentCard("VISA", "Tyrion Lannister", "4532367296473418",
                        expiredDate.Month, expiredDate.Year, 765))
            );
        }

        [Fact]
        public void Processed_WithBankId_StateChanged()
        {
            var payment = new Payment((decimal)123.56, "EUR",
                new PaymentCard("VISA", "Tyrion Lannister", "4532367296473418",
                    DateTime.Today.Month, DateTime.Today.Year, 765));

            payment.Processed("124567");

            Assert.Equal(PaymentStates.Processed, payment.State);
        }

        [Fact]
        public void Processed_WithoutBankId_FailsWithException()
        {
            var payment = new Payment((decimal) 123.56, "EUR",
                new PaymentCard("VISA", "Tyrion Lannister", "4532367296473418",
                    DateTime.Today.Month, DateTime.Today.Year, 765));

            Assert.Throws<ArgumentNullException>(() => payment.Processed(""));
        }

        [Fact]
        public void Rejected_StateChanged()
        {
            var payment = new Payment((decimal)123.56, "EUR",
                new PaymentCard("VISA", "Tyrion Lannister", "4532367296473418",
                    DateTime.Today.Month, DateTime.Today.Year, 765));

            payment.Rejected("EXPIRED");

            Assert.Equal(PaymentStates.Rejected, payment.State);
        }

    }
}
