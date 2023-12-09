using Five.Models;
using System.Text.RegularExpressions;

namespace Five;

public class Program
{
    public static void Main()
    {
        Console.WriteLine(GetSum());
    }

    private static double? GetSum()
    {
        var lines = File.ReadAllLines("input.txt");
        var mapsRows = lines
            .Select((line, i) => (lineString: line, i))
            .Where((line) => line.lineString.Contains("map"))
            .Select(l => l.i).ToArray();

        var seeds = Array.ConvertAll(lines[0].Split(": ")[1].Split(" "), s => double.Parse(s));

        var Maps = new List<Map>();
        for (int i = 0; i < mapsRows.Length; i++)
        {
            Maps.Add(ParseLinesToMap(lines[(mapsRows[i] + 1)..(mapsRows.ElementAtOrDefault(i + 1) != 0 ? mapsRows[i + 1] - 1 : lines.Length)]));
        }

        var seedList = new List<Seed>();
        for (int i = 0; i < seeds.Length; i += 2)
        {
            seedList.Add(new Seed(seeds[i], seeds[i] + seeds[i + 1] - 1));
        }

        foreach (var map in Maps)
        {
            seedList = HandleMap(map, seedList);
        }

        var result = seedList.Select(s => s.Start).Min();

        List<Seed> HandleMap(Map map, List<Seed> seedList)
        {
            var newSeedList = new List<Seed>();
            foreach (var seed in seedList)
            {
                newSeedList.AddRange(HandleSeed(map.Ranges.ToList(), seed));
            }
            return newSeedList;
        }

        List<Seed> HandleSeed(List<MapRange> ranges, Seed seed)
        {
            var partialList = new List<Seed>();

            double currentStart = seed.Start;

            for (int j = 0; j < ranges.Count; j++)
            {
                if (currentStart <= seed.End && currentStart < ranges[j].SourceStart)
                {
                    double newSeedEnd = Math.Min(seed.End, ranges[j].SourceStart - 1);
                    partialList.Add(new Seed(currentStart, newSeedEnd));
                    currentStart = newSeedEnd + 1;
                }

                if (currentStart <= seed.End && currentStart >= ranges[j].SourceStart && currentStart <= ranges[j].SourceEnd)
                {
                    double newSeedEnd = Math.Min(seed.End, ranges[j].SourceEnd);
                    partialList.Add(new Seed(ranges[j].DestinationStart + currentStart - ranges[j].SourceStart, ranges[j].DestinationStart + newSeedEnd - ranges[j].SourceStart));
                    currentStart = newSeedEnd + 1;
                }

                if (currentStart <= seed.End && j == ranges.Count - 1 && currentStart > ranges[j].SourceEnd)
                {
                    partialList.Add(new Seed(currentStart, seed.End));
                }
            }
            return partialList;
        }
        return result;
    }

    private static Map ParseLinesToMap(string[] lines)
    {
        return new Map(lines
            .Select(line => Regex.Matches(line, @"\d+"))
            .Select(matches =>
            new MapRange(
                double.Parse(matches[0].Value),
                double.Parse(matches[0].Value) + double.Parse(matches[2].Value) - 1,
                double.Parse(matches[1].Value),
                double.Parse(matches[1].Value) + double.Parse(matches[2].Value) - 1))
            .OrderBy(r => r.SourceStart));
    }
}