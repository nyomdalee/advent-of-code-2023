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

        int sum = 0;

        foreach (var line in lines)
        {
            List<Set> sets = new();
            int gameId = int.Parse(line.Split(":")[0][5..]);

            foreach (var set in line.Split(":")[1].Split(";"))
            {
                int red = 0;
                int green = 0;
                int blue = 0;

                foreach (var colorString in set.Split(","))
                {
                    if (colorString.Contains("red"))
                        red = int.Parse((colorString[..^"red".Length]));

                    if (colorString.Contains("green"))
                        green = int.Parse((colorString[..^"green".Length]));

                    if (colorString.Contains("blue"))
                        blue = int.Parse((colorString[..^"blue".Length]));
                }
                sets.Add(new Set(red, green, blue));
            }
            var maxRed = sets.Max(s => s.Red);
            var maxGreen = sets.Max(s => s.Green);
            var maxBlue = sets.Max(s => s.Blue);

            sum += maxRed * maxGreen * maxBlue;
        }
        return sum;
    }
}