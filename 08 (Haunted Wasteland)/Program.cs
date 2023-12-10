using Eight.Models;
using System.Text.RegularExpressions;

namespace Eight;

public class Program
{
    public static void Main()
    {
        Console.WriteLine(GetSum());
    }

    private static ulong GetSum()
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

        var primeFactors = startingNodes.Select(n => GetPrimeFactors(GetCount(n)));

        var commonFactors = primeFactors.Aggregate((a, b) => a.Intersect(b));
        var remainingFactors = primeFactors.SelectMany(f => f.Except(commonFactors));

        return commonFactors.Union(remainingFactors).Aggregate((a, b) => a * b);

        ulong GetCount(string node)
        {
            ulong stepCount = 0;
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

    static IEnumerable<ulong> GetPrimeFactors(ulong number)
    {
        List<ulong> factors = new();

        while (number % 2 == 0)
        {
            factors.Add(2);
            number /= 2;
        }

        for (ulong i = 3; i <= number; i += 2)
        {
            if (number == 1)
                break;

            while (number % i == 0)
            {
                factors.Add(i);
                number /= i;
            }
        }
        return factors;
    }
}