using DI.Model;

namespace DI.Descriptors;

internal class InstanceBasedServiceDescriptor : ServiceDescriptor
{
    public object Inctance { get; init; }

    public InstanceBasedServiceDescriptor(Type serviceType, object instance)
    {
        Lifetime = Lifetime.Singleton;
        ServiceType = serviceType;
        Inctance = instance;
    }
}