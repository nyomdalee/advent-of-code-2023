using Eight.Models;
using System.Text.RegularExpressions;

namespace Eight;

public class Program
{
    public static void Main()
    {
        Console.WriteLine(GetSum());
    }

    private static int GetSum()
    {
        var lines = File.ReadAllLines("input.txt");

        var directions = lines[0];

        var Nodes = lines[2..]
            .Select(line => Regex.Matches(line, @"[A-Z]+"))
            .Select(matches =>
            new Node(matches[0].Value, matches[1].Value, matches[2].Value))
            .ToDictionary(n => n.Id);

        var currentElement = "AAA";

        int GetCount()
        {
            int stepCount = 0;
            while (true)
            {
                foreach (var direction in directions)
                {
                    if (currentElement == "ZZZ")
                        return stepCount;

                    if (direction == 'L')
                        currentElement = Nodes[currentElement].Left;
                    else
                        currentElement = Nodes[currentElement].Right;

                    stepCount++;
                }
            }
        }

        return GetCount();
    }
}