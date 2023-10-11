using System.Net;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using MessagePack;
using MessagePack.Resolvers;

namespace InterfaceProxy;

public sealed class MessagePackMediaTypeFormatter : MediaTypeFormatter
{
    #region fields        
    public const string MEDIA_TYPE = "application/x-msgpack";
    private readonly MessagePackSerializerOptions _options;
    #endregion

    #region ctor
    public MessagePackMediaTypeFormatter(MessagePackSerializerOptions options = null)
    {
        SupportedMediaTypes.Add(new MediaTypeHeaderValue(MEDIA_TYPE));
        _options = options ?? ContractlessStandardResolver.Options
            .WithCompression(MessagePackCompression.Lz4BlockArray)
            .WithResolver(MessagePackResolver.Instance);
    }
    #endregion

    #region methods
    public override bool CanReadType(Type type) => true;

    public override bool CanWriteType(Type type) => true;

    public override Task<object?> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger) => Task.FromResult(MessagePackSerializer.Deserialize(type, readStream, _options));

    public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content,
        TransportContext transportContext, CancellationToken cancellationToken) => MessagePackSerializer.SerializeAsync(type, writeStream, value, _options, cancellationToken);
    #endregion
}
