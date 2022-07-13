using DI.Model;

namespace DI.Descriptors;

internal class FactoryBasedServiceDescriptor : ServiceDescriptor
{
    public Func<IScope, object> Factory { get; init; }
}