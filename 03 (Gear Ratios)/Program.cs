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

        var symbolLines = lines
            .Select((line, i) => (LineIndex: i, Positions: Regex.Matches(line, "([^0-9|.])")
            .Select(m => m.Index)));

        var numberLines = lines
            .Select((line, i) => (LineIndex: i, Numbers: Regex.Matches(line, "([0-9])+")
            .Select(m => (m.Index, Value: int.Parse(m.Value)))));

        var sum = numberLines
            .SelectMany(numberLine => numberLine.Numbers
            .Where(number => symbolLines
            .Where(symbolLine => symbolLine.LineIndex >= numberLine.LineIndex - 1 && symbolLine.LineIndex <= numberLine.LineIndex + 1)
            .SelectMany(symbol => symbol.Positions)
            .Any(symbolIndex => symbolIndex >= number.Index - 1 && symbolIndex <= number.Index + number.Value.ToString().Length))
            .Select(n => n.Value))
            .Sum();

        return sum;
    }
}