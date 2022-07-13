using DI.Model;

IContainerBuilder builder = new ContainerBuilder();
builder.RegisterSingleton<IService, Service>()
    .RegisterTransient<IHelper>(s => new Helper())
    .RegisterSingleton<IAnotherService>(AnotherServiceInstance.Instance);

class Registration
{
    public IContainer ConfigureServices()
    {
        var builder = new ContainerBuilder();
        builder.RegisterTransient<IService, Service>();
        builder.RegisterScoped<Controller, Controller>();
        return builder.Build();
    }
}

interface IAnotherService
{
}

class AnotherServiceInstance : IAnotherService
{
    private AnotherServiceInstance()
    {
    }

    public static AnotherServiceInstance Instance = new();
}

interface IHelper
{
}

class Helper : IHelper
{
}

class Controller
{
    private readonly IService _service;

    public Controller(IService service)
    {
        _service = service;
    }

    public void Do()
    {
    }
}

interface IService
{
}

class Service : IService
{
}