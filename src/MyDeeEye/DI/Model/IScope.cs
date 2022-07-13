namespace DI.Model;

public interface IScope
{
    object Resolve(Type service);
}