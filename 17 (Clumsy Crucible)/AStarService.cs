using System.Drawing;
using Seventeen.Models;

namespace Seventeen;
internal class AStarService
{
    private readonly int[,] mazeArray = { };
    private readonly int mazeWidth; /*X*/
    private readonly int mazeHeight; /*Y*/
    private readonly Point endPoint;

    private List<Node> nodesToExplore = [];
    int shortestPath = int.MaxValue;
    private int AverageTileHeat = 1;
    private int[,,,] evaluated = new int[141, 141, 4, 11];

    private static readonly Direction[] directions =
    [
        new(1, 0, 0),
        new(-1, 0, 1),
        new(0, 1, 2),
        new(0, -1, 3),
    ];

    public AStarService()
    {
        var lines = File.ReadAllLines("input.txt");

        mazeHeight = lines.Length - 1;
        mazeWidth = lines[0].Length - 1;

        endPoint = new Point(mazeWidth, mazeHeight);

        mazeArray = new int[mazeWidth + 1, mazeHeight + 1];
        for (int i = 0; i <= mazeWidth; i++)
        {
            for (int j = 0; j <= mazeHeight; j++)
            {
                mazeArray[i, j] = int.Parse(lines[j][i].ToString());
            }
        }
    }

    internal int DoAStarThings()
    {
        var initialPoint = new Point(0, 0);

        nodesToExplore.Add(new(0, initialPoint, directions.First(x => x.X == 1), 0, GetRemainingEstimate(initialPoint)));
        nodesToExplore.Add(new(0, initialPoint, directions.First(x => x.Y == 1), 0, GetRemainingEstimate(initialPoint)));

        return RunMaze();
    }

    private int RunMaze()
    {
        while (true)
        {
            if (nodesToExplore.Count == 0 || (shortestPath != 0 && nodesToExplore.Min(x => x.Heuristic) >= shortestPath))
            {
                return shortestPath;
            }

            var nextNode = nodesToExplore.MinBy(x => x.Heuristic);
            nodesToExplore.Remove(nextNode ?? throw new ArgumentNullException("dont see how this could be null but ok"));

            ExpandNode(nextNode);
            evaluated[nextNode.Position.X, nextNode.Position.Y, nextNode.Direction.Id, nextNode.Streak] = nextNode.HeatLoss;
        }

        throw new ArgumentNullException("nodes exhausted");
    }

    private void ExpandNode(Node node)
    {
        foreach (var dir in directions)
        {
            Point newPosition = new(node.Position.X + dir.X, node.Position.Y + dir.Y);

            // reverse
            if ((dir.X + node.Direction.X) == 0 && (dir.Y + node.Direction.Y) == 0)
                continue;

            // bounds
            if (newPosition.X < 0 || newPosition.Y < 0 || newPosition.X > mazeWidth || newPosition.Y > mazeHeight)
                continue;

            // min 4 forward
            if (node.Streak < 4 && !node.Direction.Equals(dir))
                continue;

            // max 10 forward
            if (node.Direction.Equals(dir) && node.Streak >= 10)
                continue;

            int newHeatLoss = node.HeatLoss + GetTileHeat(newPosition);

            if (newPosition.Equals(endPoint))
            {
                if (shortestPath > newHeatLoss)
                {
                    shortestPath = newHeatLoss;
                }
                continue;
            }

            if (newHeatLoss > shortestPath)
                continue;

            HandleNewNode(node, dir, newPosition);
        }
    }

    private void HandleNewNode(Node oldNode, Direction direction, Point newPosition)
    {
        int newStreak = direction.Equals(oldNode.Direction) ? oldNode.Streak + 1 : 1;
        int newHeatLoss = oldNode.HeatLoss + GetTileHeat(newPosition);
        int newEstimate = GetRemainingEstimate(newPosition);

        var newNode = new Node(
            HeatLoss: newHeatLoss,
            Position: newPosition,
            Direction: direction,
            Streak: newStreak,
            RemainingEstimate: newEstimate);

        var wot = evaluated[newPosition.X, newPosition.Y, direction.Id, newStreak];

        if (wot != 0)
        {
            if (wot <= newNode.HeatLoss)
                return;

            evaluated[newPosition.X, newPosition.Y, direction.Id, newStreak] = newHeatLoss;
        }

        var queuedNode = nodesToExplore
            .Where(x => x.Position.X == newPosition.X)
            .Where(x => x.Position.Y == newPosition.Y)
            .Where(x => x.Direction.X == direction.X)
            .Where(x => x.Direction.Y == direction.Y)
            .Where(x => x.Streak == newStreak)
            .FirstOrDefault();

        if (queuedNode is null)
        {
            nodesToExplore.Add(newNode);
            return;
        }

        if (queuedNode.HeatLoss > newNode.HeatLoss)
        {
            nodesToExplore.Remove(queuedNode);
            nodesToExplore.Add(newNode);
        }
    }

    private int GetTileHeat(Point point)
        => mazeArray[point.X, point.Y];

    private int GetRemainingEstimate(Point point)
        => (mazeWidth - point.X + mazeHeight - point.Y) * AverageTileHeat;
}
