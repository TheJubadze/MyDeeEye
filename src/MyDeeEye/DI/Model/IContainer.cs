using DI.Descriptors;

namespace DI.Model;

public interface IContainer
{
    IScope CreateScope();
}

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