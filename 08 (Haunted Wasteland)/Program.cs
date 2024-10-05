using System.Text.RegularExpressions;
using Eight.Models;

namespace Eight;

public class Program
{
    public static void Main()
    {
        Console.WriteLine(GetSum());
    }

    private static long GetSum()
    {
        var lines = File.ReadAllLines("input.txt");

        var directions = lines[0];

        var nodes = lines[2..]
            .Select(line => Regex.Matches(line, @"[A-Z]+"))
            .Select(matches =>
            new Node(matches[0].Value, matches[1].Value, matches[2].Value))
            .ToDictionary(n => n.Id);

        var startingNodes = nodes.Values
            .Select(n => n.Id)
            .Where(n => n[2] == 'A');

        return Utils.Utils.LCM(startingNodes.Select(GetCount).ToArray());

        long GetCount(string node)
        {
            long stepCount = 0;
            var currentNode = node;
            while (true)
            {
                foreach (var direction in directions)
                {
                    if (currentNode[2] == 'Z')
                        return stepCount;

                    currentNode = direction == 'L' ? nodes[currentNode].Left : nodes[currentNode].Right;
                    stepCount++;
                }
            }
        }
    }
}