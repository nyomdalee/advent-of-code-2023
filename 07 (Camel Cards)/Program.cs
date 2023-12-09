using Seven.Models;
using System.Text.RegularExpressions;

namespace Seven;

public class Program
{
    public static void Main()
    {
        Console.WriteLine(GetSum());
    }

    private static int GetSum()
    {
        var lines = File.ReadAllLines("input.txt");

        var Hands = lines
            .Select(line => Regex.Matches(line, @"\w+"))
            .Select(matches =>
            new Hand(matches[0].Value, int.Parse(matches[1].Value)))
            .OrderByDescending(h => h.Fitness).ToList();

        return Hands.Select((h, i) => h.Bid * (i + 1)).Sum();
    }
}