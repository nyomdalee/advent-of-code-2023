namespace Eighteen;

public static class Program
{
    public static void Main()
    {
        var service = new TrenchService();
        Console.WriteLine(service.Go());
    }
}