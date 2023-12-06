using System.Text.RegularExpressions;

namespace OneOne;
public class Program
{
    private static readonly Dictionary<string, int> _numberTable = new()
        {
            {"zero",0},{"one",1},{"two",2},{"three",3},{"four",4},
            {"five",5},{"six",6},{"seven",7},{"eight",8},{"nine",9}
        };

    public static void Main()
    {
        Console.WriteLine(GetSum());
    }

    private static int GetSum()
    {
        var lines = File.ReadAllLines($@"{AppDomain.CurrentDomain.BaseDirectory}\input.txt");
        int sum = 0;

        foreach (var line in lines)
        {
            var regexMatch = Regex.Match(line, "([0-9]|one|two|three|four|five|six|seven|eight|nine)(?:.*)([0-9]|one|two|three|four|five|six|seven|eight|nine)|([0-9])");

            var first = ConvertToNumber(regexMatch.Groups[1].Value);
            var last = ConvertToNumber(regexMatch.Groups[2].Value);
            var only = ConvertToNumber(regexMatch.Groups[3].Value);

            sum += first * 10 + last + only * 11;
        }
        return sum;
    }

    private static int ConvertToNumber(string numberString)
    {
        if (string.IsNullOrEmpty(numberString))
            return 0;

        if (int.TryParse(numberString, out int n))
            return n;

        return _numberTable[numberString];
    }
}