using System.Text.RegularExpressions;

namespace Six;

public class Program
{
    public static void Main()
    {
        Console.WriteLine(GetSum());
    }

    private static double? GetSum()
    {
        var lines = File.ReadAllLines("input.txt");

        var time = double.Parse(string.Join("", Regex.Matches(lines[0], @"\d+").Select(m => m.Value)));
        var record = double.Parse(string.Join("", Regex.Matches(lines[1], @"\d+").Select(m => m.Value)));

        double GetBoundary()
        {
            double lowerBound = 0;
            double upperBound = time;
            double midPoint;

            while (true)
            {
                midPoint = Math.Ceiling((upperBound + lowerBound) / 2);

                if (midPoint * (time - midPoint) >= record && (midPoint - 1) * (time - midPoint + 1) < record)
                    return midPoint;

                if (midPoint * (time - midPoint) > record)
                    upperBound = midPoint;

                else
                    lowerBound = midPoint;
            }
        }
        return time - ((GetBoundary() - 1) * 2) - 1;
    }
}