using Ten.Models;

namespace Ten;

public class Program
{
    public static void Main()
    {
        Console.WriteLine(GetSum());
    }

    private static long GetSum()
    {
        var lines = File.ReadAllLines("input.txt");

        var startingLine = lines.Select((line, index) => (line, i: index)).Where(_ => _.line.Contains('S')).First().i;
        var startingIndex = lines[startingLine].IndexOf('S');

        List<Coords> directions = new()
        {
            new (1, 0),
            new (0, 1),
            new (-1, 0),
            new (0, -1),

        };

        Dictionary<char, Dictionary<Coords, Coords>> fuckThis = new()
        {
            { '|', new(){{new(1, 0), new(1, 0) }, {new(-1, 0), new(-1, 0) } } },
            { '-', new(){{new(0, 1), new(0, 1) }, {new(0, -1), new(0, -1) } } },
            { 'L', new(){{new(1, 0), new(0, 1) }, {new(0, -1), new(-1, 0) } } },
            { 'J', new(){{new(1, 0), new(0, -1) }, {new(0, 1), new(-1, 0) } } },
            { '7', new(){{new(-1, 0), new(0, -1) }, {new(0, 1), new(1, 0) } } },
            { 'F', new(){{new(-1, 0), new(0, 1) }, {new(0, -1), new(1, 0) } } },
        };

        int stepCount = 0;
        RunTrail(GetFirstDirection(), new Coords(startingLine, startingIndex));
        return (stepCount - 1) / 2 + 1;

        Coords GetFirstDirection()
        {
            foreach (var direction in directions)
            {
                var character = lines![startingLine + direction.Line][startingIndex + direction.Index];

                if (character != '.' && fuckThis[character].ContainsKey(direction))
                    return direction;
            }
            throw new Exception("how did we get here");
        }

        void RunTrail(Coords direction, Coords point)
        {
            var currentDirection = direction;
            var currentPoint = point;
            while (true)
            {
                var character = lines![currentPoint.Line + currentDirection.Line][currentPoint.Index + currentDirection.Index];

                if (character == 'S')
                    return;

                stepCount++;
                currentPoint = new Coords(currentPoint.Line + currentDirection.Line, currentPoint.Index + currentDirection.Index);
                currentDirection = fuckThis[character][currentDirection];
            }
        }
    }
}