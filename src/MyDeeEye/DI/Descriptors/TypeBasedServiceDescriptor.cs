namespace DI.Descriptors;

public class TypeBasedServiceDescriptor : ServiceDescriptor
{
    public Type ImplementationType { get; init; }
}