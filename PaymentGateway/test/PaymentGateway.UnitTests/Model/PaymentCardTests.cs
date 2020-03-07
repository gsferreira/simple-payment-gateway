using System;
using System.Linq;
using FluentAssertions;
using PaymentGateway.Core.Events;
using Xunit;
using PaymentGateway.Core.Model;

namespace PaymentGateway.UnitTests.Model
{
    public class PaymentCardTests
    {
        [Fact]
        public void Create_WithValidData_Success()
        {
            var payment = new PaymentCard("VISA", "Tyrion Lannister", "4532367296473418",
                    12, DateTime.Today.Year, 765);

            Assert.NotNull(payment.Name);
            Assert.NotNull(payment.Type);
        }

        [Fact]
        public void Create_WithEmptyName_FailsWithException()
        {
            Assert.Throws<ArgumentNullException>(() => new PaymentCard("VISA", "", "4532367296473418",
                    12, DateTime.Today.Year, 765)
            );
        }

        [Fact]
        public void Create_WithInvalidCVV_FailsWithException()
        {
            Assert.Throws<ArgumentException>(() => new PaymentCard("VISA", "Tyrion Lannister", "4532367296473418",
                    12, DateTime.Today.Year, 2)
            );
        }

        [Fact]
        public void Create_WithInvalidMonth_FailsWithException()
        {
            Assert.Throws<ArgumentException>(() => new PaymentCard("VISA", "Tyrion Lannister", "4532367296473418",
                13, DateTime.Today.Year,765 )
            );
        }

    }
}
