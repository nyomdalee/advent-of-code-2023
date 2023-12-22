using Fifteen.Models;
using System.Text.RegularExpressions;

namespace Fifteen;

public class Program
{
    public static void Main()
    {
        Console.WriteLine(GetSum());
    }

    private static long GetSum()
    {
        var line = File.ReadAllText("input.txt");
        var sequenceValues = line.Split(',').ToList();

        var boxes = new Dictionary<int, List<Lens>>();
        for (int i = 0; i <= 255; i++)
            boxes.Add(i, new List<Lens>());

        for (int i = 0; i < sequenceValues.Count; i++)
        {
            var groups = Regex.Match(sequenceValues[i], "(\\w+)(=|-)(\\d?)").Groups;

            var label = groups[1].Value;
            var boxNumber = GetReindeerHash(label);

            if (groups[2].Value == "-")
            {
                // why does .Remove not have this overload?
                boxes[boxNumber].RemoveAll(l => l.Label == label);
            }
            else
            {
                if (!boxes[boxNumber].Any(l => l.Label == label))
                    boxes[boxNumber].Add(new Lens(label, int.Parse(groups[3].Value)));
                else
                    boxes[boxNumber][boxes[boxNumber].FindIndex(l => l.Label == label)] = new Lens(label, int.Parse(groups[3].Value));
            }
        }

        return boxes
            .SelectMany((b, i) => b.Value
            .Select((l, j) => (i + 1) * (j + 1) * l.FocalLength))
            .Sum();
    }

    private static int GetReindeerHash(string inputString)
    {
        int currentValue = 0;
        for (int i = 0; i < inputString.Length; i++)
        {
            var asciiValue = (int)inputString[i];
            currentValue += asciiValue;
            currentValue *= 17;
            currentValue %= 256;
        }
        return currentValue;
    }
}