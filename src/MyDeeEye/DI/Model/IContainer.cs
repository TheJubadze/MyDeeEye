namespace DI.Model;

internal interface IContainer
{
    IScope CreateScope();
}