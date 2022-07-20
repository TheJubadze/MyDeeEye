namespace Playground;

class AnotherServiceInstance : IAnotherService
{
    private AnotherServiceInstance()
    {
    }

    public static AnotherServiceInstance Instance = new();
}