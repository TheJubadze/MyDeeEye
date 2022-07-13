using DI.Model;

namespace DI.Descriptors;

internal abstract class ServiceDescriptor
{
    public Type ServiceType { get; init; }
    public Lifetime Lifetime { get; init; }
}