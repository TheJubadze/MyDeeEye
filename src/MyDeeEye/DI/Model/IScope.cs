namespace DI.Model;

internal interface IScope
{
    object Resolve(Type service);
}