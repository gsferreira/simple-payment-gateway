using System.Threading.Tasks;
using PaymentGateway.Core.Model;

namespace PaymentGateway.Application.BankProviders
{
    public interface IAcquiringBank
    {
        Task<BankPaymentResult> Process(Payment payment);
    }
}
