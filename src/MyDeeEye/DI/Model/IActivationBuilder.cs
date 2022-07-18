using DI.Descriptors;

namespace DI.Model;

public interface IActivationBuilder
{
    Func<IScope, object> BuildActivation(ServiceDescriptor descriptor);
}