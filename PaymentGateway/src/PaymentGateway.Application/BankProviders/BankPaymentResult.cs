namespace PaymentGateway.Application.BankProviders
{
    public class BankPaymentResult
    {
        public bool Success { get; set; }
        public string Id { get; set; }
        public string Message { get; set; }
    }
}
