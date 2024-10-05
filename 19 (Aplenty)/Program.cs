namespace Nineteen;

public static class Program
{
    public static void Main()
    {
        var service = new SortingService();
        Console.WriteLine(service.Go());
    }
}