using DI.Descriptors;

namespace DI.Model;

internal interface IContainerBuilder
{
    void Register(ServiceDescriptor serviceDescriptor);
    IContainer Build();
}