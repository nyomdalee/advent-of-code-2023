using System.Runtime.InteropServices;
using Twelve.Models;

namespace Twelve;

public class Program
{
    public static void Main()
    {
        Console.WriteLine(GetSum());
    }

    private static long GetSum()
    {
        var springsLines = File.ReadAllLines("input.txt")
            .Select(line =>
            {
                var parts = line.Split(' ');
                var text = parts[0];
                var damagedGroups = parts[1].Split(',').Select(int.Parse).ToArray();

                text = string.Concat(Enumerable.Repeat(text + "?", 4)) + text;
                damagedGroups = [.. Enumerable.Repeat(damagedGroups, 5).SelectMany(arr => arr)];

                return new SpringLine(text, damagedGroups);
            })
            .ToList();

        return springsLines.Sum(GetArrangmentCount);
    }

    private static void AddOperational(Dictionary<ThumbprintKey, long> cache, KeyValuePair<ThumbprintKey, long> kvp) =>
        AddOrUpdate(cache, kvp.Key with { IsActive = false, Length = kvp.Key.Length + 1 }, kvp.Value);

    private static void AddDamaged(Dictionary<ThumbprintKey, long> cache, KeyValuePair<ThumbprintKey, long> kvp, int[] expectedGroups)
    {
        var newPrint = new List<int>(kvp.Key.Thumbprint);
        if (!kvp.Key.IsActive || kvp.Key.Thumbprint.Length == 0)
        {
            newPrint.Add(1);
        }
        else
        {
            newPrint[^1]++;
        }

        if (IsValidRunningThumbprint(CollectionsMarshal.AsSpan(newPrint), expectedGroups))
        {
            AddOrUpdate(cache, kvp.Key with { Thumbprint = [.. newPrint], IsActive = true, Length = kvp.Key.Length + 1 }, kvp.Value);
        }
    }

    private static long GetArrangmentCount(SpringLine springLine)
    {
        var text = springLine.Text;

        Dictionary<ThumbprintKey, long> currentCache = new()
        {
            [new ThumbprintKey([], false, 0)] = 1
        };

        for (int i = 0; i < text.Length; i++)
        {
            Dictionary<ThumbprintKey, long> nextCache = [];

            foreach (var kvp in currentCache)
            {
                if (text[i] == '.')
                {
                    AddOperational(nextCache, kvp);
                }
                else if (text[i] == '#')
                {
                    AddDamaged(nextCache, kvp, springLine.DamagedGroups);
                }
                else
                {
                    AddOperational(nextCache, kvp);
                    AddDamaged(nextCache, kvp, springLine.DamagedGroups);
                }
            }

            if (i == text.Length - 1)
            {
                return nextCache
                    .Select(x => (x.Key.Thumbprint, x.Value))
                    .Where(s => IsValidFinalThumbprint(s.Thumbprint, springLine.DamagedGroups))
                    .Sum(x => x.Value);
            }
            currentCache = nextCache;
        }
        throw new Exception("Should not reach here");
    }

    private static bool IsValidRunningThumbprint(ReadOnlySpan<int> current, ReadOnlySpan<int> damagedGroups)
    {
        if (current.Length > damagedGroups.Length)
        {
            return false;
        }

        for (int i = 0; i < current.Length - 1; i++)
        {
            if (current[i] != damagedGroups[i])
            {
                return false;
            }
        }

        return current.Length <= 0 || current[^1] <= damagedGroups[current.Length - 1];
    }

    private static bool IsValidFinalThumbprint(int[] current, int[] damagedGroups) =>
        current.Length == damagedGroups.Length && current.SequenceEqual(damagedGroups);

    private static void AddOrUpdate(Dictionary<ThumbprintKey, long> cache, ThumbprintKey key, long count)
    {
        if (!cache.TryAdd(key, count))
        {
            cache[key] += count;
        }
    }
}