using System.Text;

namespace Fourteen;

public class Program
{
    public static void Main()
    {
        Console.WriteLine(GetSum());
    }

    private static long GetSum()
    {
        var lines = File.ReadAllLines("input.txt").ToList();
        var tiltedLines = RotateLines(lines).Select(TiltLine);

        return tiltedLines.Select(l => l.Select((c, i) => c == 'O' ? l.Count - i : 0).Sum()).Sum();
    }

    private static List<char> TiltLine(string line)
    {
        var oldLine = line.ToCharArray().ToList();
        var currentLine = oldLine.ToList();
        while (true)
        {
            for (int i = 1; i < currentLine.Count; i++)
            {
                if (currentLine[i] != 'O')
                    continue;

                if (currentLine[i - 1] == '.')
                {
                    currentLine[i - 1] = 'O';
                    currentLine[i] = '.';
                }
            }
            if (oldLine.SequenceEqual(currentLine))
                break;

            oldLine = currentLine.ToList();
        }
        return currentLine;
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