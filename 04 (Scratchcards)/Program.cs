using Four.Models;

namespace Four;

public class Program
{
    public static void Main()
    {
        Console.WriteLine(GetSum());
    }

    private static int GetSum()
    {
        var lines = File.ReadAllLines("input.txt");

        var splitOptions = StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries;

        var cards = lines.Select(cardString => new Card(int.Parse(cardString.Split(":")[0].Split(" ", splitOptions)[1]),
            Array.ConvertAll(cardString.Split(":")[1].Split("|")[0].Split(" ", splitOptions), s => int.Parse(s)),
            Array.ConvertAll(cardString.Split(":")[1].Split("|")[1].Split(" ", splitOptions), s => int.Parse(s))));

        var sum = cards
            .Select(card => card.WinningNumbers.Intersect(card.HeldNumbers))
            .Select(matchedNumbers => matchedNumbers.Any() ? Math.Pow(2, matchedNumbers.Count() - 1) : 0)
            .Sum();

        return (int)sum;
    }
}