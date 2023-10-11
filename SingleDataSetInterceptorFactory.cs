using System.Net.Http.Formatting;
using Castle.DynamicProxy;

namespace InterfaceProxy;

public sealed class SingleDataSetInterceptorFactory : IInterceptorFactory
{
    private static readonly HttpClient _httpClient;
    private static readonly MediaTypeFormatter _sendFormatter;
    private static readonly MediaTypeFormatter[] _receiveFormatters;
    private const string BASE_ADDRESS = "https://some.remote.server";

    static SingleDataSetInterceptorFactory()
    {
        _httpClient = new() { BaseAddress = new Uri(BASE_ADDRESS) };
        _sendFormatter = new MessagePackMediaTypeFormatter();
        _receiveFormatters = [new MessagePackMediaTypeFormatter(), new JsonMediaTypeFormatter()];
    }

    public IInterceptor Create()
    {
        return (IInterceptor)Activator.CreateInstance(typeof(SingleDataSetInterceptor),[ _httpClient, _sendFormatter, _receiveFormatters])!;
    }
}
