using DI.Model;

namespace Playground;

class Registration
{
    public IContainer ConfigureServices()
    {
        var builder = new ContainerBuilder(new LambdaBasedActivationBuilder());
        builder.RegisterTransient<IService, Service>();
        builder.RegisterScoped<Controller, Controller>();
        return builder.Build();
    }
}