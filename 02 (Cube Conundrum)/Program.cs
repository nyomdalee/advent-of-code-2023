using Two.Models;

namespace Two;
public class Program
{
    public static void Main()
    {
        Console.WriteLine(GetSum());
    }

    private static int GetSum()
    {
        var lines = File.ReadAllLines("input.txt");
        List<Game> games = new();

        foreach (var line in lines)
        {
            int gameId = 0;
            List<Set> sets = new();

            var gameString = line.Split(":");
            gameId = int.Parse(gameString[0].Substring(5));

            var setStrings = gameString[1].Split(";");

            foreach (var set in setStrings)
            {
                int red = 0;
                int green = 0;
                int blue = 0;

                var colors = set.Split(",");

                foreach (var colorString in colors)
                {
                    var trimmedString = colorString.Trim();

                    if (trimmedString.Contains("red"))
                        red = int.Parse((trimmedString.Substring(0, trimmedString.Length - "red".Length - 1)).Trim());

                    if (trimmedString.Contains("green"))
                        green = int.Parse((trimmedString.Substring(0, trimmedString.Length - "green".Length - 1)).Trim());

                    if (trimmedString.Contains("blue"))
                        blue = int.Parse((trimmedString.Substring(0, trimmedString.Length - "blue".Length - 1).Trim()));
                }
                sets.Add(new Set(red, green, blue));
            }

            games.Add(new Game(gameId, sets));
        }

        var invalidGames = games
            .Where(g => g.Sets
            .Any(s => s.Red > 12 || s.Green > 13 || s.Blue > 14));

        var validGames = games.Except(invalidGames);
        return validGames.Select(g => g.Id).Sum();
    }
}