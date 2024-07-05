namespace Seventeen;

public static class Program
{
    public static void Main()
    {
        var service = new AStarService();
        Console.WriteLine(service.DoAStarThings());
    }
}