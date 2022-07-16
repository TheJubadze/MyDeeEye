namespace DI.Model;

public interface IContainer : IDisposable, IAsyncDisposable
{
    IScope CreateScope();
}