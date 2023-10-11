namespace InterfaceProxy;

[AttributeUsage(AttributeTargets.Interface)]
public class HttpInterceptorAttribute : Attribute
{
    public HttpInterceptorAttribute(Type interceptorFactoryType)
    {
        if(!interceptorFactoryType.IsAssignableTo(typeof(IInterceptorFactory)))
        {
            throw new InvalidCastException($"{interceptorFactoryType} is not assignable to {typeof(IInterceptorFactory)}");
        }
        InterceptorFactoryType = interceptorFactoryType;
    }

    public Type InterceptorFactoryType { get; }
}

[AttributeUsage(AttributeTargets.Interface)]
public sealed class HttpInterceptorAttribute<T> : HttpInterceptorAttribute
    where T : IInterceptorFactory
{
    public HttpInterceptorAttribute() : base(typeof(T))
    {
    }
}