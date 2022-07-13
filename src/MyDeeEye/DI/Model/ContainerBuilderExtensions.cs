using DI.Descriptors;

namespace DI.Model;

internal static class ContainerBuilderExtensions
{
    public static IContainerBuilder
        RegisterSingleton(this IContainerBuilder builder, Type service, Type implementation) =>
        RegisterType(builder, service, implementation, Lifetime.Singleton);

    public static IContainerBuilder
        RegisterTransient(this IContainerBuilder builder, Type service, Type implementation) =>
        RegisterType(builder, service, implementation, Lifetime.Transient);

    public static IContainerBuilder
        RegisterScoped(this IContainerBuilder builder, Type service, Type implementation) =>
        RegisterType(builder, service, implementation, Lifetime.Scoped);


    public static IContainerBuilder RegisterSingleton<TService, TImplementation>(this IContainerBuilder builder)
        where TImplementation : TService =>
        RegisterType(builder, typeof(TService), typeof(TImplementation), Lifetime.Singleton);

    public static IContainerBuilder RegisterTransient<TService, TImplementation>(this IContainerBuilder builder)
        where TImplementation : TService =>
        RegisterType(builder, typeof(TService), typeof(TImplementation), Lifetime.Transient);

    public static IContainerBuilder RegisterScoped<TService, TImplementation>(this IContainerBuilder builder)
        where TImplementation : TService =>
        RegisterType(builder, typeof(TService), typeof(TImplementation), Lifetime.Scoped);


    public static IContainerBuilder RegisterSingleton<TService>(this IContainerBuilder builder,
        Func<IScope, TService> factory) =>
        RegisterFactory(builder, typeof(TService), s => factory(s), Lifetime.Singleton);

    public static IContainerBuilder RegisterTransient<TService>(this IContainerBuilder builder,
        Func<IScope, TService> factory) =>
        RegisterFactory(builder, typeof(TService), s => factory(s), Lifetime.Transient);

    public static IContainerBuilder RegisterScoped<TService>(this IContainerBuilder builder,
        Func<IScope, TService> factory) =>
        RegisterFactory(builder, typeof(TService), s => factory(s), Lifetime.Scoped);


    public static IContainerBuilder
        RegisterSingleton(this IContainerBuilder builder, Type service, object instance) =>
        RegisterInstance(builder, service, instance);


    private static IContainerBuilder RegisterType(this IContainerBuilder builder,
        Type service,
        Type implementation,
        Lifetime lifetime)
    {
        builder.Register(new TypeBasedServiceDescriptor
            { ImplementationType = implementation, Lifetime = lifetime, ServiceType = service });

        return builder;
    }

    private static IContainerBuilder RegisterFactory(this IContainerBuilder builder,
        Type service,
        Func<IScope, object> factory,
        Lifetime lifetime)
    {
        builder.Register(new FactoryBasedServiceDescriptor
            { Factory = factory, Lifetime = lifetime, ServiceType = service });

        return builder;
    }

    private static IContainerBuilder RegisterInstance(this IContainerBuilder builder, Type service, object instance)
    {
        builder.Register(new InstanceBasedServiceDescriptor(service, instance));

        return builder;
    }
}