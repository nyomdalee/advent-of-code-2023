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

        var times = Regex.Matches(lines[0], @"\d+").Select(m => int.Parse(m.Value)).ToList();
        var records = Regex.Matches(lines[1], @"\d+").Select(m => int.Parse(m.Value)).ToList();

        int total = 1;
        for (int i = 0; i < times.Count; i++)
        {
            int raceTotal = 0;
            for (int j = 0; j <= times[i]; j++)
            {
                if (j * (times[i] - j) > records[i])
                    raceTotal++;
            }
            total *= raceTotal;
        }
        return total;
    }
}