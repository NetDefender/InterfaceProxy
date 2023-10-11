using System.Data;
using System.Net.Http.Formatting;
using Castle.DynamicProxy;

namespace InterfaceProxy;

public class SingleDataSetInterceptor : IInterceptor
{
    private readonly HttpClient _httpClient;
    private readonly MediaTypeFormatter _httpSendFormatter;
    private readonly MediaTypeFormatter[] _httpReceiveFormatters;

    public SingleDataSetInterceptor(HttpClient httpClient, MediaTypeFormatter httpSendFormatter, MediaTypeFormatter[] httpReceiveFormatters)
    {
        _httpClient = httpClient;
        _httpSendFormatter = httpSendFormatter;
        _httpReceiveFormatters = httpReceiveFormatters;
    }
    public void Intercept(IInvocation invocation)
    {
        invocation.ReturnValue = InvokeHttpPostAsync(invocation.Method.DeclaringType?.FullName, invocation.Method.Name
            , (DataSet)invocation.GetArgumentValue(0), invocation.Method.ReturnType);
    }

    private async Task<DataSet> InvokeHttpPostAsync(string? classPath, string methodName, DataSet? parameter, Type returnType)
    {
        ArgumentNullException.ThrowIfNull(classPath);
        ArgumentNullException.ThrowIfNull(methodName);
        ArgumentNullException.ThrowIfNull(parameter);

        using HttpResponseMessage response = await _httpClient.PostAsync($"/{classPath}/{methodName}"
            , new ObjectContent(parameter.GetType(), parameter, _httpSendFormatter));
        response.EnsureSuccessStatusCode();

        return (DataSet)await response.Content.ReadAsAsync(returnType, _httpReceiveFormatters);
    }
}