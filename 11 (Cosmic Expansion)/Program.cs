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
        var expandedRows = GetExpandedRows(lines);
        var expandedColumns = GetExpandedColumns(lines);

        var galaxies = lines
        .SelectMany((l, i) => Regex.Matches(l, "#")
        .Select(m => new Point(i, m.Index))).ToList();

        return GetSumOfDistances(galaxies, expandedRows, expandedColumns);
    }

    static List<int> GetExpandedRows(List<string> lines)
    {
        List<int> expandedRows = new();

        for (int i = 0; i < lines.Count; i++)
        {
            if (!lines[i].Contains('#'))
                expandedRows.Add(i);
        }
        return expandedRows;
    }

    static List<int> GetExpandedColumns(List<string> lines)
    {
        List<int> expandedColumns = new();

        for (int i = 0; i < lines[0].Length; i++)
        {
            if (!lines.Select(l => l[i]).Any(c => c == '#'))
                expandedColumns.Add(i);
        }
        return expandedColumns;
    }

    static long GetSumOfDistances(List<Point> galaxies, List<int> expandedRows, List<int> expandedColumns)
    {
        int expansionCoefficient = 1000000;
        long sumOfPaths = 0;
        for (int i = 0; i < galaxies.Count; i++)
        {
            for (int j = i + 1; j < galaxies.Count; j++)
            {
                // This can definitely be simplified, but I've run out of braincells for the day.
                int bigX = galaxies[i].X > galaxies[j].X ? galaxies[i].X : galaxies[j].X;
                int smolX = galaxies[i].X < galaxies[j].X ? galaxies[i].X : galaxies[j].X;

                int bigY = galaxies[i].Y > galaxies[j].Y ? galaxies[i].Y : galaxies[j].Y;
                int smolY = galaxies[i].Y < galaxies[j].Y ? galaxies[i].Y : galaxies[j].Y;

                int basePath = (bigX - smolX) + (bigY - smolY);

                int crossedRows = expandedRows.Where(r => r > smolX && r < bigX).Count();
                int crossedColumns = expandedColumns.Where(c => c > smolY && c < bigY).Count();

                long dilatedPath = (crossedRows + crossedColumns) * (expansionCoefficient - 1);

                sumOfPaths += basePath + dilatedPath;
            }
        }
        return sumOfPaths;
    }
}