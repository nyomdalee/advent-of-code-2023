using System.Drawing;
using System.Text;
using Eighteen.Models;

namespace Eighteen;

public class TrenchService
{
    private const char Trench = '#';
    private const char Unresolved = '.';
    private const char Empty = ' ';

    private int maxX = 0;
    private int maxY = 0;

    private static readonly Direction[] directions =
    [
        new(1, 0, 'R'),
        new(-1, 0, 'L'),
        new(0, -1, 'U'),
        new(0, 1, 'D'),
    ];

    public int Go()
    {
        var lines = File.ReadAllLines("input.txt");
        List<Instruction> instructions = [];

        for (int i = 0; i < lines.Length; i++)
        {
            string? line = lines[i];
            var parts = line.Split(' ');
            instructions.Add(new(
                directions.First(x => x.Id == char.Parse(parts[0])),
                int.Parse(parts[1]),
                parts[2]));
        }

        var currentPoint = new Point(0, 0);
        List<Point> points = [currentPoint];

        foreach (var instr in instructions)
        {
            for (int i = 0; i < instr.Distance; i++)
            {
                var newPoint = new Point(currentPoint.X + instr.Direction.X, currentPoint.Y + instr.Direction.Y);
                points.Add(newPoint);
                currentPoint = newPoint;
            }
        }
        return Process(points);
    }

    public int Process(List<Point> originalPoints)
    {
        var xOffset = Math.Abs(originalPoints.Min(x => x.X));
        var yOffset = Math.Abs(originalPoints.Min(x => x.Y));

        maxX = originalPoints.Max(x => x.X) + xOffset + 1;
        maxY = originalPoints.Max(x => x.Y) + yOffset + 1;

        List<Node> nodes = [];
        for (int x = 0; x < maxX; x++)
        {
            for (int y = 0; y < maxY; y++)
            {
                var exists = originalPoints.Any(p => p.X + xOffset == x && p.Y + yOffset == y);

                var value = exists ? Trench : Unresolved;
                nodes.Add(new(x, y, value));
            }
        }

        EvaluateUnresolved(nodes);
        Visualize(nodes, maxY, maxX);

        return nodes.Count(x => x.Value == Trench);
    }

    private void EvaluateUnresolved(List<Node> nodes)
    {
        while (true)
        {
            var nextNode = nodes.FirstOrDefault(x => x.Value == Unresolved);
            if (nextNode is null)
                break;

            ExpandGroup(nodes, nextNode);
        }
    }

    private void ExpandGroup(List<Node> nodes, Node initialNode)
    {
        List<Node> groupNodes = [];
        List<Node> nodesToExpand = [initialNode];
        bool isOuter = false;

        while (nodesToExpand.Count > 0)
        {
            var nextNode = nodesToExpand[0];

            foreach (var direction in directions)
            {
                var newX = nextNode.X + direction.X;
                var newY = nextNode.Y + direction.Y;

                if (newX < 0 || newX >= maxX || newY < 0 || newY >= maxY)
                {
                    isOuter = true;
                    continue;
                }

                var neighbor = nodes.Where(x => x.X == newX && x.Y == newY).FirstOrDefault();
                if (neighbor!.Value == Empty)
                {
                    isOuter = true;
                    continue;
                }

                if (neighbor.Value == Unresolved && !groupNodes.Contains(neighbor) && !nodesToExpand.Contains(neighbor))
                {
                    nodesToExpand.Add(neighbor);
                }
            }
            groupNodes.Add(nextNode);
            nodesToExpand.Remove(nextNode);
        }

        foreach (var node in groupNodes)
        {
            node.Value = isOuter ? Empty : Trench;
        }
    }

    private static void Visualize(List<Node> nodes, int maxY, int maxX)
    {
        var sb = new StringBuilder();
        for (int y = 0; y < maxY; y++)
        {
            for (int x = 0; x < maxX; x++)
            {
                sb.Append(nodes.First(n => n.X == x && n.Y == y).Value);
            }
            sb.AppendLine();
        }

        File.WriteAllText("output.txt", sb.ToString());
    }
}