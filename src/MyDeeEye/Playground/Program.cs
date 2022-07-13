using DI.Model;

IContainerBuilder builder = new ContainerBuilder();
builder.RegisterSingleton<IService, Service>()
    .RegisterTransient<IHelper>(s => new Helper())
    .RegisterSingleton<IAnotherService>(AnotherServiceInstance.Instance);
    
    