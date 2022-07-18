using DI.Model;

IContainerBuilder builder = new ContainerBuilder(new LambdaBasedActivationBuilder());

using (var container = builder.RegisterTransient<IService, Service>()
           .RegisterScoped<Controller, Controller>()
           .RegisterSingleton<IAnotherService>(AnotherServiceInstance.Instance)
           .Build())
{
    var scope = container.CreateScope();
    var controller1 = scope.Resolve(typeof(Controller));
    var controller2 = scope.Resolve(typeof(Controller));
    var scope2 = container.CreateScope();
    var controller3 = scope2.Resolve(typeof(Controller));
    var i1 = scope.Resolve(typeof(IAnotherService));
    var i2 = scope2.Resolve(typeof(IAnotherService));

    if (controller1 != controller2)
    {
        throw new InvalidOperationException();
    }
}

Console.WriteLine("Success");