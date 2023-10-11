using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using MessagePack;
using MessagePack.Formatters;
using MessagePack.Resolvers;

namespace InterfaceProxy;

public sealed class MessagePackResolver : IFormatterResolver
{
    #region fields
    private static readonly ReadOnlyCollection<IFormatterResolver> _resolvers = new(new IFormatterResolver[]
    {
        NativeDecimalResolver.Instance, NativeGuidResolver.Instance, NativeDateTimeResolver.Instance, TypelessContractlessStandardResolver.Instance
    });
    #endregion

    #region properties        
    public static readonly MessagePackResolver Instance = new();

    public readonly Dictionary<Type, IMessagePackFormatter> PriorityFormatters;
    #endregion

    #region ctor
    private MessagePackResolver()
    {
        PriorityFormatters = [];
    }
    #endregion

    #region methods        
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IMessagePackFormatter<T> GetFormatter<T>() => Cache<T>.Formatter;
    #endregion

    #region classes        
    private static class Cache<T>
    {
        #region ctor            
        static Cache()
        {
            if (Instance.PriorityFormatters.TryGetValue(typeof(T), out IMessagePackFormatter? customFormatter))
            {
                Formatter = (IMessagePackFormatter<T>)customFormatter;
                return;
            }

            for (int i = 0; i < _resolvers.Count; i++)
            {
                IMessagePackFormatter<T>? formatter = _resolvers[i].GetFormatter<T>();
                if (formatter is not null)
                {
                    Formatter = formatter;
                    return;
                }
            }
        }
        #endregion

        #region properties            
        public static readonly IMessagePackFormatter<T> Formatter;
        #endregion
    }
    #endregion
}
