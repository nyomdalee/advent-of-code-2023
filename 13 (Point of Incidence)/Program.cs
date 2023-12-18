using System.Text;
using Thirteen.Modles;

namespace Thirteen;

public class Program
{
    public static void Main()
    {
        Console.WriteLine(GetSum());
    }

    private static long GetSum()
    {
        var maps = File.ReadAllText("input.txt").Split("\r\n\r\n").Select(p => p.Split("\r\n").ToList()).ToList();
        return maps.Select(GetReflectionSum).Sum();
    }

    private static int GetReflectionSum(List<string> mapLines)
    {
        var rotatedLines = RotateLines(mapLines);

        var vertical = ValidateReflection(GetReflectionCandidates(mapLines[0]), mapLines);
        var horizontal = ValidateReflection(GetReflectionCandidates(rotatedLines[0]), rotatedLines);

        return vertical.Select(v => v.Index).Sum() + horizontal.Select(v => v.Index).Sum() * 100;

        List<Reflection> ValidateReflection(List<Reflection> possibleReflections, List<string> lines)
        {
            List<Reflection> valid = new();

            foreach (var reflection in possibleReflections)
            {
                if (lines.All(IsValidLine))
                    valid.Add(reflection);

                bool IsValidLine(string line)
                {
                    var left = line[0..reflection.Index].Reverse().ToArray();
                    var right = line[reflection.Index..].ToArray();

                    for (int j = 0; j < reflection.Length; j++)
                    {
                        if (left.Length < reflection.Length - 1 || right.Length < reflection.Length - 1 || left[j] != right[j])
                            return false;
                    }
                    return true;
                }
            }
            return valid;
        }

        List<Reflection> GetReflectionCandidates(string line)
        {
            List<Reflection> reflectionCandidates = new();

            for (int i = 1; i < line.Length; i++)
            {
                var left = line[0..i].Reverse().ToArray();
                var right = line[i..].ToArray();

                int length = 0;
                for (int j = 0; j < (right.Length > left.Length ? left.Length : right.Length); j++)
                {
                    if (left[j] != right[j])
                        break;

                    length++;
                }

                if (length > 0 && (i - length == 0 || length + i == line.Length))
                    reflectionCandidates.Add(new Reflection(i, length));
            }
            return reflectionCandidates;
        }
    }

    private static List<string> RotateLines(List<string> mapLines)
    {
        List<string> rotatedLines = new();

        for (int i = 0; i < mapLines[0].Length; i++)
        {
            var sb = new StringBuilder();
            for (int j = 0; j < mapLines.Count; j++)
            {
                sb.Append(mapLines[j][i]);
            }
            rotatedLines.Add(sb.ToString());
        }
        return rotatedLines;
    }
}