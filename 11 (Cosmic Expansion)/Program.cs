using System.Drawing;
using System.Text.RegularExpressions;

namespace Eleven;

public class Program
{
    public static void Main()
    {
        Console.WriteLine(GetSum());
    }

    private static long GetSum()
    {
        var lines = File.ReadAllLines("input.txt").ToList();
        var expandedUniverse = ExpandColumns(ExpandRows(lines));

        var galaxies = expandedUniverse
        .SelectMany((l, i) => Regex.Matches(l, "#")
        .Select(m => new Point(i, m.Index))).ToList();

        return GetSumOfDistances(galaxies);
    }

    static List<string> ExpandRows(List<string> lines)
    {
        List<string> expandedRows = new(lines);
        int offset = 0;

        for (int i = 0; i < lines.Count; i++)
        {
            if (!lines[i].Contains('#'))
            {
                expandedRows.Insert(i + offset, lines[i]);
                offset++;
            }
        }
        return expandedRows;
    }

    static List<string> ExpandColumns(List<string> lines)
    {
        int offset = 0;
        var expandedColumns = lines.Select(l => l.ToList()).ToList();

        for (int i = 0; i < lines[0].Length; i++)
        {
            if (!lines.Select(l => l[i]).Any(c => c == '#'))
            {
                for (int j = 0; j < lines.Count; j++)
                    expandedColumns[j].Insert(i + offset, '.');

                offset++;
            }
        }
        return expandedColumns.Select(l => new string(l.ToArray())).ToList();
    }

    static int GetSumOfDistances(List<Point> galaxies)
    {
        int sumOfPaths = 0;
        for (int i = 0; i < galaxies.Count; i++)
        {
            for (int j = i + 1; j < galaxies.Count; j++)
                sumOfPaths += Math.Abs(galaxies[i].X - galaxies[j].X) + Math.Abs(galaxies[i].Y - galaxies[j].Y);
        }
        return sumOfPaths;
    }
}