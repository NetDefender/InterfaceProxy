namespace InterfaceProxy;

[ServiceOrigin(Origin.Http)]
[HttpInterceptor<SingleDataSetInterceptorFactory>]
public interface IPayment : ILegacyMarkerService
{
    Task<Payment> Checkout(Basket basket);
}