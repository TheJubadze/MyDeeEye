using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using DI.Model;
using Playground;

BenchmarkRunner.Run<ContainerBenchmark>();

namespace Playground
{
    public class ContainerBenchmark
    {
        private readonly IScope _reflectionBased;
        private readonly IScope _lambdaBased;

        public ContainerBenchmark()
        {
            var reflectionBasedBuilder = new ContainerBuilder(new LambdaBasedActivationBuilder());
            var lambdaBasedBuilder = new ContainerBuilder(new ReflectionBasedActivationBuilder());
            InitContainer(reflectionBasedBuilder);
            InitContainer(lambdaBasedBuilder);
            _reflectionBased = reflectionBasedBuilder.Build().CreateScope();
            _lambdaBased = lambdaBasedBuilder.Build().CreateScope();
        }

        private void InitContainer(ContainerBuilder builder)
        {
            builder.RegisterTransient<IService, Service>()
                .RegisterTransient<Controller, Controller>();
        }

        [Benchmark(Baseline = true)]
        public Controller Create() => new Controller(new Service());

        [Benchmark]
        public Controller Reflection() => (Controller)_reflectionBased.Resolve(typeof(Controller));

        [Benchmark]
        public Controller Lambda() => (Controller)_lambdaBased.Resolve(typeof(Controller));
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
