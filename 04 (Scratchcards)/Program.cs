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
            Array.ConvertAll(cardString.Split(":")[1].Split("|")[1].Split(" ", splitOptions), s => int.Parse(s))))
            .OrderBy(c => c.Id).ToList();

        var cardCount = cards.Select(c => (c.Id - 1, 1)).ToDictionary();

        for (var i = 0; i < cards.Count(); i++)
        {
            var matchedNumbers = cards[i].WinningNumbers.Intersect(cards[i].HeldNumbers);

            for (var j = matchedNumbers.Count(); j > 0; j--)
            {
                if (i + j < cards.Count)
                    cardCount[i + j] += cardCount[i];
            }
        }
        return cardCount.Select(c => c.Value).Sum();
    }
}