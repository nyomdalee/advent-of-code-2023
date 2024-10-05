namespace Twenty;

public static class Program
{
    public static void Main()
    {
        var service = new ModuleService();
        Console.WriteLine(service.Go());
    }
}