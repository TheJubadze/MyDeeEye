using DI.Model;

IContainerBuilder builder = new ContainerBuilder();
var container = builder.RegisterTransient<IService, Service>()
    .RegisterScoped<Controller, Controller>()
    .Build();

var scope = container.CreateScope();
var controller = scope.Resolve(typeof(Controller));
Console.WriteLine("Success");