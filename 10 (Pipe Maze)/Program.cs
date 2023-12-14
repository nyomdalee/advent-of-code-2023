using Ten.Models;

namespace Ten;

public class Program
{
    // This entire solution is awful and needs to be completely re-tought.
    // ..perhaps when I have more time.
    public static void Main()
    {
        Console.WriteLine(GetSum());
    }

    private static readonly List<Coords> directions = new()
        {
            new (1, 0),
            new (0, 1),
            new (-1, 0),
            new (0, -1),
        };

    private static readonly Dictionary<char, Dictionary<Coords, Coords>> pipeDictionary = new()
        {
            { '|', new(){{new(1, 0), new(1, 0) }, {new(-1, 0), new(-1, 0) } } },
            { '-', new(){{new(0, 1), new(0, 1) }, {new(0, -1), new(0, -1) } } },
            { 'L', new(){{new(1, 0), new(0, 1) }, {new(0, -1), new(-1, 0) } } },
            { 'J', new(){{new(1, 0), new(0, -1) }, {new(0, 1), new(-1, 0) } } },
            { '7', new(){{new(0, 1), new(1, 0) }, { new(-1, 0), new(0, -1) }, } },
            { 'F', new(){{new(-1, 0), new(0, 1) }, {new(0, -1), new(1, 0) } } },
        };

    private static long GetSum()
    {
        var lines = File.ReadAllLines("input.txt");
        var pipeLines = lines.ToList();

        var startingLine = lines.Select((line, index) => (line, i: index)).Where(_ => _.line.Contains('S')).First().i;
        var startingIndex = lines[startingLine].IndexOf('S');

        int stepCount = 0;
        RunTrail(GetFirstDirection(), new Coords(startingLine, startingIndex));
        return PartTwo(lines.ToList(), pipeLines, stepCount);

        // Part 1
        //return (stepCount - 1) / 2 + 1;

        Coords GetFirstDirection()
        {
            foreach (var direction in directions)
            {
                var character = lines![startingLine + direction.Line][startingIndex + direction.Index];

                if (character != '.' && pipeDictionary[character].ContainsKey(direction))
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

                XMarksTheSpot(currentPoint);

                if (character == 'S')
                    return;

                stepCount++;
                currentPoint = new Coords(currentPoint.Line + currentDirection.Line, currentPoint.Index + currentDirection.Index);
                currentDirection = pipeDictionary[character][currentDirection];
            }
        }

        void XMarksTheSpot(Coords point)
        {
            char[] characters = pipeLines[point.Line].ToCharArray();
            characters[point.Index] = 'X';
            pipeLines[point.Line] = new string(characters);
        }
    }

    //
    //  If youre reading this, close this file while you still can.
    //  Below lies the most unhinged code ever written.
    //  Proceed at your own peril.
    //
    static int PartTwo(List<string> originalLines, List<string> pipeLines, int stepCount)
    {
        pipeLines[0] = ORow(pipeLines[0]);

        var charLines = pipeLines.Select(l => l.ToCharArray()).ToList();
        // It's a christmas tree !
        for (var x = 0; x < pipeLines.Count; x++)
        {
            for (var i = 0; i < charLines.Count; i++)
            {
                for (var j = 0; j < charLines[i].Length; j++)
                {
                    if (charLines[i][j] != 'X' && charLines[i][j] != 'O')
                    {
                        foreach (var direction in directions)
                        {
                            var checkPos = new Coords(i + direction.Line, j + direction.Index);
                            if (checkPos.Line < 0 || checkPos.Index < 0 || checkPos.Line >= charLines.Count || checkPos.Index >= charLines[i].Length)
                                continue;

                            if (charLines[checkPos.Line][checkPos.Index] == 'O')
                                charLines[i][j] = 'O';
                        }
                    }
                }
            }
        }

        List<Coords> islands = new();

        for (var i = 0; i < charLines.Count; i++)
        {
            for (var j = 0; j < charLines[i].Length; j++)
            {
                if (charLines[i][j] != 'X' && charLines[i][j] != 'O')
                {
                    charLines[i][j] = 'H';
                    islands.Add(new(i, j));
                }

                if (charLines[i][j] == 'X')
                {
                    charLines[i][j] = originalLines[i][j];
                }

                //HACK: fix this
                if (charLines[i][j] == 'S')
                    charLines[i][j] = '7';
            }
        }

        var yepLines = charLines.Select(c => new string(c)).ToList();
        int escapedCount = 0;

        foreach (var island in islands)
        {
            var startPoint = GetStartPoint(island);
            var startDirection = pipeDictionary[yepLines[startPoint.Line][startPoint.Index]].Values.Last();
            EscapeTrail(startDirection, startPoint);
        }

        return islands.Count - escapedCount;

        Coords GetStartPoint(Coords island)
        {
            for (var i = island.Index; i >= 0; i--)
            {
                if (pipeLines[island.Line][i] == 'X')
                    return new(island.Line, i);
            }

            throw new Exception("how?");
        }

        bool EscapeTrail(Coords direction, Coords point)
        {
            var currentDirection = direction;
            var currentPoint = point;
            for (var i = 0; i < stepCount; i++)
            {
                var character = yepLines![currentPoint.Line + currentDirection.Line][currentPoint.Index + currentDirection.Index];

                var currentCharacter = yepLines![currentPoint.Line][currentPoint.Index];

                if (currentCharacter == '|' && currentDirection == new Coords(-1, 0) && yepLines[currentPoint.Line][currentPoint.Index + 1] == 'O')
                {
                    escapedCount++;
                    return true;
                }

                currentPoint = new Coords(currentPoint.Line + currentDirection.Line, currentPoint.Index + currentDirection.Index);
                currentDirection = pipeDictionary[character][currentDirection];
            }
            return false;
        }
    }

    static string ORow(string line)
    {
        char[] characters = line.ToCharArray();
        for (int i = 0; i < characters.Length; i++)
        {
            if (characters[i] != 'X')
                characters[i] = 'O';
        }
        return new string(characters);
    }
}