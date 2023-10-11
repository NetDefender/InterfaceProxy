namespace InterfaceProxy;

[AttributeUsage(AttributeTargets.Interface)]
public sealed class ServiceOriginAttribute : Attribute
{
    public ServiceOriginAttribute(Origin origin)
    {
        Origin = origin;
    }

    public Origin Origin { get; }
}