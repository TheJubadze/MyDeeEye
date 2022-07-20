using System.Reflection;
using DI.Descriptors;

namespace DI.Model;

public class ReflectionBasedActivationBuilder : ActivationBuilderBase
{
    protected override Func<IScope, object> BuildActivationInternal(TypeBasedServiceDescriptor tb,
        ConstructorInfo ctor,
        ParameterInfo[] args,
        ServiceDescriptor descriptor)
    {
        return scope =>
        {
            var argsForCtor = args.Select(arg => scope.Resolve(arg.ParameterType)).ToArray();

            return ctor.Invoke(argsForCtor);
        };
    }
}