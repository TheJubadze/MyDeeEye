using DI.Model;

namespace DI.Descriptors;

public abstract class ServiceDescriptor
{
    public Type ServiceType { get; init; }
    public Lifetime Lifetime { get; init; }
}