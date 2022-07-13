using DI.Descriptors;

namespace DI.Model;

public class ContainerBuilder : IContainerBuilder
{
    private readonly List<ServiceDescriptor> _descriptors = new();

    public void Register(ServiceDescriptor serviceDescriptor)
    {
        _descriptors.Add(serviceDescriptor);
    }

    public IContainer Build()
    {
        throw new NotImplementedException();
    }
}