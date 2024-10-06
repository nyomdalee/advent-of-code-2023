namespace TwentyOne;

public static class Program
{
    public static void Main()
    {
        var service = new StepService();
        Console.WriteLine(service.Go());
    }
}