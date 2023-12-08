using Five.Models;
using System.Text.RegularExpressions;

namespace Five;

public class Program
{
    public static void Main()
    {
        Console.WriteLine(GetSum());
    }

    private static double GetSum()
    {
        var lines = File.ReadAllLines("input.txt");
        var mapsRows = lines
            .Select((line, i) => (lineString: line, i))
            .Where((line) => line.lineString.Contains("map"))
            .Select(l => l.i).ToArray();

        var seeds = Array.ConvertAll(lines[0].Split(": ")[1].Split(" "), s => double.Parse(s));

        var allMaps = new List<Map>();

        for (int i = 0; i < mapsRows.Length; i++)
        {
            allMaps.Add(ParseLinesToMap(lines[(mapsRows[i] + 1)..(mapsRows.ElementAtOrDefault(i + 1) != 0 ? mapsRows[i + 1] - 1 : lines.Length - 1)]));
        }

        var values = seeds.ToList();

        foreach (var map in allMaps)
        {
            var newValues = new List<double>();

            foreach (var value in values)
            {
                var current = value;

                foreach (var mapRange in map.Ranges)
                {
                    if (value >= mapRange.Source && value < mapRange.Source + mapRange.Length)
                        current = mapRange.Destination + value - mapRange.Source;
                }
                newValues.Add(current);
            }
            values = newValues;
        }

        return values.Min();
    }

    private static Map ParseLinesToMap(string[] lines)
    {
        return new Map(lines
            .Select(line => Regex.Matches(line, @"\d+"))
            .Select(matches => new MapRange(double.Parse(matches[0].Value), double.Parse(matches[1].Value), double.Parse(matches[2].Value))));
    }
}