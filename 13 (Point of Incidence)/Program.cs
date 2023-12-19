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

        var verticalCandidates = GetReflectionCandidates(mapLines[0]).Concat(GetReflectionCandidates(mapLines[1])).Distinct().ToList();
        var horizontalCandidates = GetReflectionCandidates(rotatedLines[0]).Concat(GetReflectionCandidates(rotatedLines[1])).Distinct().ToList();

        var vertical = ValidateReflections(verticalCandidates, mapLines);
        var horizontal = ValidateReflections(horizontalCandidates, rotatedLines);

        return vertical.Select(v => v.Index).Sum() + horizontal.Select(v => v.Index).Sum() * 100;

        // This doesn't actually check whether the smudge on a possible horizontal match is the same one as one on a possible vertical match.
        // In other words, this is a very bad "solution". but it works because the inputs are playing nice.
        List<Reflection> ValidateReflections(List<Reflection> possibleReflections, List<string> lines)
        {
            List<Reflection> valid = new();

            foreach (var reflection in possibleReflections)
                ValidateReflection(reflection);

            void ValidateReflection(Reflection reflection)
            {
                int smudgeCount = 0;

                foreach (var line in lines)
                {
                    var left = line[0..reflection.Index].Reverse().ToArray();
                    var right = line[reflection.Index..].ToArray();

                    for (int j = 0; j < reflection.Length; j++)
                    {
                        if (left.Length < reflection.Length - 1 || right.Length < reflection.Length - 1 || left[j] != right[j])
                        {
                            if (smudgeCount < 1)
                                smudgeCount++;
                            else
                                return;
                        }
                    }
                }

                if (smudgeCount == 1)
                    valid.Add(reflection);
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