using Castle.DynamicProxy;

namespace InterfaceProxy;

public interface IInterceptorFactory
{
    IInterceptor Create();
}
