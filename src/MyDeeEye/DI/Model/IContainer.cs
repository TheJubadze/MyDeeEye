namespace DI.Model;

public interface IContainer
{
    IScope CreateScope();
}