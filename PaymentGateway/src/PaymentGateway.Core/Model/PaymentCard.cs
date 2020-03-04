using PaymentGateway.SharedKernel;
using System.Collections.Generic;

namespace PaymentGateway.Core.Model
{
    public class PaymentCard : ValueObject
    {
        public PaymentCard(string type, string name, long number, int expireMonth, int expireYear, int cvv)
        {
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
        public int ExpireMonth { get;  }
        public int  ExpireYear { get;  }
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
