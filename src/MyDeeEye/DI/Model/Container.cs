using DI.Descriptors;

namespace DI.Model;

internal class Container : IContainer
{
    public Container(IEnumerable<ServiceDescriptor> descriptors)
    {
    }

    public IScope CreateScope()
    {
        throw new NotImplementedException();
    }
}