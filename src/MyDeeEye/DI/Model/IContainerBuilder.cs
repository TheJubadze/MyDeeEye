using DI.Descriptors;

namespace DI.Model;

public interface IContainerBuilder
{
    void Register(ServiceDescriptor serviceDescriptor);
    IContainer Build();
}