using Sixteen.Models;
using System.Drawing;
using System.Numerics;

namespace Sixteen;

public class Program
{
    public static void Main()
    {
        Console.WriteLine(GetSum());
    }

    private static long GetSum()
    {
        var lines = File.ReadAllLines("input.txt");

        var mazeArray = new char[lines.Length, lines[0].Length];
        for (int i = 0; i < lines.Length; i++)
            for (int j = 0; j < lines[0].Length; j++)
                mazeArray[i, j] = lines[i][j];

        List<Beam> initialBeams = new List<Beam>();
        for (int i = 0; i < mazeArray.GetLength(0); i++)
        {
            initialBeams.Add(new Beam(new Point(i, 0), new Vector2(0, 1)));
            initialBeams.Add(new Beam(new Point(i, mazeArray.GetLength(1) - 1), new Vector2(0, -1)));
        }

        for (int i = 0; i < mazeArray.GetLength(1); i++)
        {
            initialBeams.Add(new Beam(new Point(0, i), new Vector2(1, 0)));
            initialBeams.Add(new Beam(new Point(mazeArray.GetLength(1) - 1, i), new Vector2(-1, 0)));
        }

        return initialBeams.Select(EvaluateBeam).Max();

        int EvaluateBeam(Beam initialBeam)
        {
            var beamHistory = new List<Beam>();
            var beamList = new List<Beam>() { initialBeam };

            while (beamList.Count > 0)
            {
                foreach (var beam in beamList)
                    if (!beamHistory.Contains(beam))
                        beamHistory.Add(beam);

                beamList = beamList.SelectMany(InteractWithMaze).Except(beamHistory).ToList();
            }
            Console.WriteLine(initialBeam.ToString());
            return beamHistory.Select(b => b.NextPosition).Distinct().Count();
        }

        // Polymorphism? What is that?
        List<Beam> InteractWithMaze(Beam beam)
        {
            List<Beam> newBeams = new();
            void PassThrough() => newBeams.Add(new Beam(new Point(beam.NextPosition.X + (int)beam.Direction.X, beam.NextPosition.Y + (int)beam.Direction.Y), beam.Direction));

            var obstacle = mazeArray[beam.NextPosition.X, beam.NextPosition.Y];
            switch (obstacle)
            {
                case '.':
                    PassThrough();
                    break;
                case '-':
                    if (beam.Direction.X == 0)
                        PassThrough();
                    else
                    {
                        newBeams.Add(new Beam(new Point(beam.NextPosition.X, beam.NextPosition.Y + 1), new Vector2(0, 1)));
                        newBeams.Add(new Beam(new Point(beam.NextPosition.X, beam.NextPosition.Y - 1), new Vector2(0, -1)));
                    }
                    break;
                case '|':
                    if (beam.Direction.Y == 0)
                        PassThrough();
                    else
                    {
                        newBeams.Add(new Beam(new Point(beam.NextPosition.X + 1, beam.NextPosition.Y), new Vector2(1, 0)));
                        newBeams.Add(new Beam(new Point(beam.NextPosition.X - 1, beam.NextPosition.Y), new Vector2(-1, 0)));
                    }
                    break;
                case '\\':
                    if (beam.Direction.X == -1)
                        newBeams.Add(new Beam(new Point(beam.NextPosition.X, beam.NextPosition.Y - 1), new Vector2(0, -1)));
                    if (beam.Direction.X == 1)
                        newBeams.Add(new Beam(new Point(beam.NextPosition.X, beam.NextPosition.Y + 1), new Vector2(0, 1)));
                    if (beam.Direction.Y == 1)
                        newBeams.Add(new Beam(new Point(beam.NextPosition.X + 1, beam.NextPosition.Y), new Vector2(1, 0)));
                    if (beam.Direction.Y == -1)
                        newBeams.Add(new Beam(new Point(beam.NextPosition.X - 1, beam.NextPosition.Y), new Vector2(-1, 0)));
                    break;
                case '/':
                    if (beam.Direction.X == -1)
                        newBeams.Add(new Beam(new Point(beam.NextPosition.X, beam.NextPosition.Y + 1), new Vector2(0, 1)));
                    if (beam.Direction.X == 1)
                        newBeams.Add(new Beam(new Point(beam.NextPosition.X, beam.NextPosition.Y - 1), new Vector2(0, -1)));
                    if (beam.Direction.Y == 1)
                        newBeams.Add(new Beam(new Point(beam.NextPosition.X - 1, beam.NextPosition.Y), new Vector2(-1, 0)));
                    if (beam.Direction.Y == -1)
                        newBeams.Add(new Beam(new Point(beam.NextPosition.X + 1, beam.NextPosition.Y), new Vector2(1, 0)));
                    break;
            }
            return newBeams.Where(b => b.NextPosition.X >= 0 && b.NextPosition.X < mazeArray.GetLength(0) && b.NextPosition.Y >= 0 && b.NextPosition.Y < mazeArray.GetLength(1)).ToList();
        }
    }
}