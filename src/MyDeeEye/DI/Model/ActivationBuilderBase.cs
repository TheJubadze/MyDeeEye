﻿using System.Reflection;
using DI.Descriptors;

namespace DI.Model;

public abstract class ActivationBuilderBase : IActivationBuilder
{
    public Func<IScope, object> BuildActivation(ServiceDescriptor descriptor)
    {
        var tb = (TypeBasedServiceDescriptor)descriptor;
        var ctor = tb.ImplementationType.GetConstructors(BindingFlags.Public | BindingFlags.Instance).Single();
        var args = ctor.GetParameters();

        return BuildActivationInternal(tb, ctor, args, descriptor);
    }

    protected abstract Func<IScope, object> BuildActivationInternal(TypeBasedServiceDescriptor tb,
        ConstructorInfo ctor,
        ParameterInfo[] args,
        ServiceDescriptor descriptor);
}