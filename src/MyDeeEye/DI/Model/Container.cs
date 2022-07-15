using System.Reflection;
using DI.Descriptors;

namespace DI.Model;

internal class Container : IContainer
{
    private class Scope : IScope
    {
        private readonly Container _container;

        public Scope(Container container)
        {
            _container = container;
        }

        public object Resolve(Type service) => _container.CreateInstance(service, this);
    }

    private readonly Dictionary<Type, ServiceDescriptor> _descriptors;

    public Container(IEnumerable<ServiceDescriptor> descriptors)
    {
        _descriptors = descriptors.ToDictionary(x => x.ServiceType);
    }

    public IScope CreateScope()
    {
        return new Scope(this);
    }

    private object CreateInstance(Type service, IScope scope)
    {
        if (!_descriptors.TryGetValue(service, out var descriptor))
        {
            throw new InvalidOperationException($"Service {service} is not registered");
        }

        if (descriptor is InstanceBasedServiceDescriptor ib)
        {
            return ib.Inctance;
        }

        if (descriptor is FactoryBasedServiceDescriptor fb)
        {
            return fb.Factory(scope);
        }

        var tb = (TypeBasedServiceDescriptor)descriptor;

        var ctor = tb.ImplementationType.GetConstructors(BindingFlags.Public | BindingFlags.Instance).Single();
        var args = ctor.GetParameters();
        var argsForCtor = args.Select(arg => CreateInstance(arg.ParameterType, scope)).ToArray();

        return ctor.Invoke(argsForCtor);
    }
}