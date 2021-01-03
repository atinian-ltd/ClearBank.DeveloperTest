namespace ClearBank.DeveloperTest.Services
{
    public interface IPaymentServiceFactory
    {
        IPaymentService BuildPaymentService();
    }
}