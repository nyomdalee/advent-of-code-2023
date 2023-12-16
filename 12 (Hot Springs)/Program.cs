using System.Text.RegularExpressions;
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
        var lines = File.ReadAllLines("input.txt").ToList();
        List<SpringLine> springsLines = lines
            .Select(l => new SpringLine(l.Split(' ')[0],
            Array.ConvertAll(l.Split(' ')[1].Split(","), s => int.Parse(s)))).ToList();

        return springsLines.Select(l => GetArrangmentCount(l)).Sum();
    }

    static int GetArrangmentCount(SpringLine springLine)
    {
        return GetPermutations(springLine.Text)
            .Where(p => Regex.IsMatch(p, GetRegex(springLine.DamagedGroups))).Count();
    }

    static List<string> GetPermutations(string text)
    {
        List<string> currentSet = new() { string.Empty };

        for (int i = 0; i < text.Length; i++)
        {
            List<string> newSet = new();

            foreach (string s in currentSet)
            {
                if (text[i] == '.' || text[i] == '#')
                    newSet.Add($"{s}{text[i]}");

                else
                {
                    newSet.Add($"{s}#");
                    newSet.Add($"{s}.");
                }
            }
            currentSet = newSet;
        }
        return currentSet.Select(s => s.Contains('#') ? s[s.IndexOf('#')..(s.LastIndexOf('#') + 1)] : s).ToList();
    }

    static string GetRegex(int[] damagedGroups)
    {
        return $"^{string.Join(@"\.+", damagedGroups.Select(g => $"#{{{g}}}"))}$";
    }
}