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
        var charLines = RotateToInitialPosition(lines).Select(l => l.ToCharArray().ToList());

        var requestedCycles = 1000000000;
        (var start, var end) = FindLoop(charLines);

        var actualCycles = start + (requestedCycles - start) % (end - start);
        return RunIt(charLines, actualCycles);
    }

    private static int RunIt(IEnumerable<List<char>> lines, int cycles)
    {
        var centrifugeLines = lines.ToList();
        for (int i = 0; i < cycles; i++)
            centrifugeLines = Cycle(centrifugeLines);

        return centrifugeLines.Select(l => l.Select((c, i) => c == 'O' ? l.Count - i : 0).Sum()).Sum();
    }

    private static List<List<char>> Cycle(List<List<char>> lines)
    {
        for (int j = 0; j < 4; j++)
        {
            lines = lines.Select(TiltCharArray).ToList();
            lines = Rotate90Clockwise(lines.ToList());
        }
        return lines;
    }

    private static (int LoopStart, int LoopEnd) FindLoop(IEnumerable<List<char>> lines)
    {
        // ghetto guids
        List<string> positionStrings = new();

        var centrifugeLines = lines.ToList();
        var oldLines = centrifugeLines.ToList();
        int i = 0;

        while (true)
        {
            centrifugeLines = Cycle(centrifugeLines);

            if (i == 125)
                File.WriteAllLines("125.txt", Rotate90Clockwise(centrifugeLines.ToList()).Select(l => new string(l.ToArray())));

            if (i == 184)
                File.WriteAllLines("184.txt", Rotate90Clockwise(centrifugeLines.ToList()).Select(l => new string(l.ToArray())));

            var dumboString = centrifugeLines.Select(l => string.Join("", l)).Aggregate((total, part) => $"{total} {part}");
            if (positionStrings.Contains(dumboString))
                return (positionStrings.IndexOf(dumboString), i);
            else
                positionStrings.Add(dumboString);

            oldLines = centrifugeLines.ToList();
            i++;
        }
    }

    private static List<char> TiltCharArray(List<char> line)
    {
        var oldLine = line.ToList();
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

    private static List<List<char>> Rotate90Clockwise(List<List<char>> mapLines)
    {
        List<List<char>> rotatedLines = new();

        for (int i = 0; i < mapLines[0].Count; i++)
        {
            var sb = new StringBuilder();
            for (int j = mapLines.Count - 1; j >= 0; j--)
            {
                sb.Append(mapLines[j][i]);
            }
            rotatedLines.Add(sb.ToString().ToCharArray().ToList());
        }
        return rotatedLines;
    }

    private static List<string> RotateToInitialPosition(List<string> mapLines)
    {
        List<string> rotatedLines = new();

        for (int j = mapLines[0].Length - 1; j >= 0; j--)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < mapLines.Count; i++)
            {
                sb.Append(mapLines[i][j]);
            }
            rotatedLines.Add(sb.ToString());
        }
        return rotatedLines;
    }
}