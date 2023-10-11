using System.Reflection;
using Castle.DynamicProxy;

namespace InterfaceProxy;

public static class RemotingServiceFactory
{
    private static readonly Dictionary<Type, IInterceptor> _httpServices;
    private static readonly ProxyGenerator _generator;

    static RemotingServiceFactory()
    {
        _generator = new ProxyGenerator();
        _httpServices = [];

        foreach (Type? type in typeof(RemotingServiceFactory)
            .Assembly
            .GetExportedTypes()
            .Where(x => x.GetCustomAttribute<ServiceOriginAttribute>() is not null))
        {
            ServiceOriginAttribute serviceOrigin = type.GetCustomAttribute<ServiceOriginAttribute>()!;
            if(serviceOrigin.Origin == Origin.Http)
            {
                HttpInterceptorAttribute interceptorAtribute = type.GetCustomAttribute<HttpInterceptorAttribute>()!;
                IInterceptorFactory interceptorFactory = (IInterceptorFactory)Activator.CreateInstance(interceptorAtribute.InterceptorFactoryType)!;
                _httpServices.Add(type, interceptorFactory.Create());
            }
        }
    }

    public static T GetService<T>() where T : class
    {
        if (_httpServices.TryGetValue(typeof(T), out IInterceptor? interceptor))
        {
            return GetHttpService<T>(interceptor);
        }
        else
        {
            return GetRemotingService<T>();
        }
    }

    private static T GetHttpService<T>(IInterceptor interceptor) where T :  class
    {
        return _generator.CreateInterfaceProxyWithoutTarget<T>(interceptor);
    }

    private static T GetRemotingService<T>() where T : class
    {
        // Old code that returns a reference from Remoting
        // ...
        // ...
        // Here we return default for simplicity
        return default!;
    }
}
