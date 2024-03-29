using DI.Descriptors;

namespace DI.Model;

public class ContainerBuilder : IContainerBuilder
{
    private readonly List<ServiceDescriptor> _descriptors = new();
    private readonly IActivationBuilder _builder;

    public ContainerBuilder(IActivationBuilder builder)
    {
        _builder = builder;
    }

    public void Register(ServiceDescriptor serviceDescriptor)
    {
        _descriptors.Add(serviceDescriptor);
    }

    public IContainer Build()
    {
        return new Container(_descriptors, _builder);
    }
}