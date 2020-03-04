using System;
using PaymentGateway.SharedKernel;
using System.Collections.Generic;

namespace PaymentGateway.Core.Model
{
    public class PaymentCard : ValueObject
    {
        public PaymentCard(string type, string name, long number, int expireMonth, int expireYear, int cvv)
        {
            if (string.IsNullOrWhiteSpace(type)) throw new ArgumentNullException(nameof(type));
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
            if (cvv < 100 || cvv > 999) throw new ArgumentException("CVV must have 3 digits.", nameof(cvv));
            if (expireMonth < 1 || expireMonth > 12) throw new ArgumentException("Invalid card expire month", nameof(expireMonth));

            Type = type;
            Name = name;
            Number = number;
            ExpireMonth = expireMonth;
            ExpireYear = expireYear;
            CVV = cvv;
        }

        public string Type { get; }
        public string Name { get; }
        public long Number { get; }
        public int ExpireMonth { get; }
        public int ExpireYear { get; }
        public int CVV { get; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Type;
            yield return Name;
            yield return Number;
            yield return ExpireMonth;
            yield return ExpireYear;
            yield return CVV;
        }
    }

}
