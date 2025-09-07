using Eighteen.Models;

namespace Eighteen;

public static class Program
{
    private static readonly Direction[] directions =
    [
    new(1, 0, '0'),
        new(-1, 0, '2'),
        new(0, -1, '1'),
        new(0, 1, '3'),
    ];

    public static void Main()
    {
        Console.WriteLine(Go());
    }

    public static long Go()
    {
        var lines = File.ReadAllLines("input.txt");
        List<Instruction> instructions = [];

        for (int i = 0; i < lines.Length; i++)
        {
            string? line = lines[i];
            var hex = line.Substring(line.IndexOf('#') + 1, 6);
            instructions.Add(new(
                directions.First(x => x.Id == hex[^1]),
                Convert.ToInt64(hex[0..^1], 16)));
        }

        var currentNode = new Node(0, 0);
        List<Node> nodes = [currentNode];
        long perimeter = 0;

        for (int i = 0; i < instructions.Count; i++)
        {
            var instr = instructions[i];

            var newVertex = new Node(
                currentNode.X + (instr.Direction.X * instr.Distance),
                currentNode.Y + (instr.Direction.Y * instr.Distance));

            perimeter += instr.Distance;
            nodes.Add(newVertex);
            currentNode = newVertex;
        }

        return CalculateArea(nodes, perimeter);
    }

    public static long CalculateArea(List<Node> vertices, long perimeter)
    {
        long area = 0;
        for (int j = 0; j < vertices.Count - 1; j++)
        {
            area += (vertices[j].X * vertices[j + 1].Y) - (vertices[j].Y * vertices[j + 1].X);
        }

        area = Math.Abs(area);

        return ((area + perimeter) / 2) + 1;
    }
}