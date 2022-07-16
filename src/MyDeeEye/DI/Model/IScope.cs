namespace DI.Model;

public interface IScope : IDisposable, IAsyncDisposable
{
    object Resolve(Type service);
}