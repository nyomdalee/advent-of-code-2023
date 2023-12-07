using System.Text.RegularExpressions;

namespace Three;

public record Suck(int lineId, IEnumerable<int> positions);
public class Program
{
    public static void Main()
    {
        Console.WriteLine(GetSum());
    }

    private static int GetSum()
    {
        var lines = File.ReadAllLines("input.txt");

        var stars = lines
            .SelectMany((line, i) => Regex.Matches(line, @"\*")
            .Select(m => (Line: i, m.Index)));

        var numbers = lines
            .SelectMany((line, i) => Regex.Matches(line, "[0-9]+")
            .Select(m => (Line: i, m.Index, Value: int.Parse(m.Value))));

        var sum = stars
            .Select(star => numbers
            .Where(number => number.Line >= star.Line - 1 && number.Line <= star.Line + 1 && star.Index >= number.Index - 1 && star.Index <= number.Index + number.Value.ToString().Length)
            .Select(n => n.Value))
            .Where(matchingNumbers => matchingNumbers.Count() == 2)
            .Select(num => num.ToList()[0] * num.ToList()[1])
            .Sum();

        return sum;
    }
}