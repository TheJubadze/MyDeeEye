using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Reflection;
using DI.Descriptors;

namespace DI.Model;

internal class Container : IContainer
{
    private class Scope : IScope
    {
        private readonly Container _container;
        private readonly ConcurrentDictionary<Type, object> _scopedInstances = new();
        private readonly ConcurrentStack<object> _disposables = new();

        public Scope(Container container)
        {
            _container = container;
        }

        public object Resolve(Type service)
        {
            var descriptor = _container.FindDescriptor(service);
            if (descriptor?.Lifetime == Lifetime.Transient)
            {
                return CreateInstanceInternal(service);
            }

            if (descriptor?.Lifetime == Lifetime.Scoped || _container._rootScope == this)
            {
                return _scopedInstances.GetOrAdd(service, CreateInstanceInternal);
            }

            return _container._rootScope.Resolve(service);
        }

        public void Dispose()
        {
            foreach (var disposable in _disposables)
            {
                if (disposable is IDisposable d)
                {
                    d.Dispose();
                }
                else if (disposable is IAsyncDisposable ad)
                {
                    ad.DisposeAsync().GetAwaiter().GetResult();
                }
            }
        }

        public async ValueTask DisposeAsync()
        {
            foreach (var disposable in _disposables)
            {
                if (disposable is IDisposable d)
                {
                    d.Dispose();
                }
                else if (disposable is IAsyncDisposable ad)
                {
                    await ad.DisposeAsync();
                }
            }
        }

        private object CreateInstanceInternal(Type service)
        {
            var result = _container.CreateInstance(service, this);
            if (result is IDisposable or IAsyncDisposable)
            {
                _disposables.Push(result);
            }

            return result;
        }
    }

    private readonly ImmutableDictionary<Type, ServiceDescriptor> _descriptors;
    private readonly ConcurrentDictionary<Type, Func<IScope, object>> _builtActivators = new();
    private readonly Scope _rootScope;

    public Container(IEnumerable<ServiceDescriptor> descriptors)
    {
        _descriptors = descriptors.ToImmutableDictionary(x => x.ServiceType);
        _rootScope = new(this);
    }

    public IScope CreateScope()
    {
        return new Scope(this);
    }

    public void Dispose()
    {
        _rootScope.Dispose();
    }

    public ValueTask DisposeAsync()
    {
        return _rootScope.DisposeAsync();
    }

    private ServiceDescriptor? FindDescriptor(Type service)
    {
        _descriptors.TryGetValue(service, out var result);
        return result;
    }

    private Func<IScope, object> BuildActivation(Type service)
    {
        if (!_descriptors.TryGetValue(service, out var descriptor))
        {
            throw new InvalidOperationException($"Service {service} is not registered");
        }

        if (descriptor is InstanceBasedServiceDescriptor ib)
        {
            return _ => ib.Inctance;
        }

        if (descriptor is FactoryBasedServiceDescriptor fb)
        {
            return fb.Factory;
        }

        var tb = (TypeBasedServiceDescriptor)descriptor;

        var ctor = tb.ImplementationType.GetConstructors(BindingFlags.Public | BindingFlags.Instance).Single();
        var args = ctor.GetParameters();

        return scope =>
        {
            var argsForCtor = args.Select(arg => CreateInstance(arg.ParameterType, scope)).ToArray();

            return ctor.Invoke(argsForCtor);
        };
    }

    private object CreateInstance(Type service, IScope scope)
    {
        return _builtActivators.GetOrAdd(service, BuildActivation)(scope);
    }
}