using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FakeBank.Models
{
    public class RetrieveModel
    {
        public decimal Amount { get; set; }
        public string CardName { get; set; }
        public long CardNumber { get; set; }
        public int CardExpireMonth { get; set; }
        public int CardExpireYear { get; set; }
        public int CardCVV { get; set; }
    }
}
