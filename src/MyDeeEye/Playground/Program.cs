using Autofac;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using DI.Model;
using Microsoft.Extensions.DependencyInjection;
using Playground;
using ContainerBuilder = DI.Model.ContainerBuilder;

BenchmarkRunner.Run<ContainerBenchmark>();

namespace Playground
{
    [MemoryDiagnoser]
    public class ContainerBenchmark
    {
        private readonly IScope _reflectionBased;
        private readonly IScope _lambdaBased;
        private readonly ILifetimeScope _autofac;
        private readonly IServiceScope _msDi;

        public ContainerBenchmark()
        {
            var reflectionBasedBuilder = new ContainerBuilder(new ReflectionBasedActivationBuilder());
            var lambdaBasedBuilder = new ContainerBuilder(new LambdaBasedActivationBuilder());
            InitContainer(reflectionBasedBuilder);
            InitContainer(lambdaBasedBuilder);
            _reflectionBased = reflectionBasedBuilder.Build().CreateScope();
            _lambdaBased = lambdaBasedBuilder.Build().CreateScope();

            _autofac = InitAutofac(new Autofac.ContainerBuilder());
            _msDi = InitMsDi(new ServiceCollection());
        }

        private static void InitContainer(ContainerBuilder builder)
        {
            builder.RegisterTransient<IService, Service>()
                .RegisterTransient<Controller, Controller>();
        }

        private static ILifetimeScope InitAutofac(Autofac.ContainerBuilder builder)
        {
            builder.RegisterType<Service>().As<IService>();
            builder.RegisterType<Controller>().AsSelf();

            return builder.Build().BeginLifetimeScope();
        }

        private static IServiceScope InitMsDi(IServiceCollection collection)
        {
            collection.AddTransient<IService, Service>();
            collection.AddTransient<Controller, Controller>();

            return collection.BuildServiceProvider().CreateScope();
        }


        [Benchmark(Baseline = true)]
        public Controller Create() => new Controller(new Service());

        [Benchmark]
        public Controller Reflection() => (Controller)_reflectionBased.Resolve(typeof(Controller));

        [Benchmark]
        public Controller Lambda() => (Controller)_lambdaBased.Resolve(typeof(Controller));

        [Benchmark]
        public Controller Autofac() => _autofac.Resolve<Controller>();

        [Benchmark]
        public Controller MsDi() => _msDi.ServiceProvider.GetRequiredService<Controller>();
    }
}


// IContainerBuilder builder = new ContainerBuilder(new LambdaBasedActivationBuilder());
//
// using (var container = builder.RegisterTransient<IService, Service>()
//            .RegisterScoped<Controller, Controller>()
//            .RegisterSingleton<IAnotherService>(AnotherServiceInstance.Instance)
//            .Build())
// {
//     var scope = container.CreateScope();
//     var controller1 = scope.Resolve(typeof(Controller));
//     var controller2 = scope.Resolve(typeof(Controller));
//     var scope2 = container.CreateScope();
//     var controller3 = scope2.Resolve(typeof(Controller));
//     var i1 = scope.Resolve(typeof(IAnotherService));
//     var i2 = scope2.Resolve(typeof(IAnotherService));
//
//     if (controller1 != controller2)
//     {
//         throw new InvalidOperationException();
//     }
// }
//
// Console.WriteLine("Success");