using TwentyOne.Models;

namespace TwentyOne;
internal class StepService
{
    private const int StepCount = 64;

    private static readonly Direction[] directions =
    [
        new(1, 0),
        new(-1, 0),
        new(0, -1),
        new(0, 1),
    ];

    public long Go()
    {
        var currentGrid = ParseIntput();
        var cleanGrid = DeepCleanCopyCharArray(currentGrid);

        for (int i = 0; i < StepCount; i++)
        {
            var newGrid = DeepCleanCopyCharArray(cleanGrid);

            StepOnIt(currentGrid, newGrid);
            //DebugPrint(newGrid);
            currentGrid = newGrid;
        }

        return CountIt(currentGrid);
    }

    private int CountIt(char[,] currentGrid)
    {
        int rows = currentGrid.GetLength(0);
        int columns = currentGrid.GetLength(1);

        int count = 0;
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                if (Equals(currentGrid[x, y], 'O'))
                {
                    count++;
                }
            }
        }
        return count;
    }

    private void StepOnIt(char[,] currentGrid, char[,] newGrid)
    {
        int rows = currentGrid.GetLength(0);
        int columns = currentGrid.GetLength(1);

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                var currentChar = currentGrid[x, y];

                if (!Equals(currentChar, 'O') && !Equals(currentChar, 'S'))
                {
                    continue;
                }

                foreach (var direction in directions)
                {
                    Paint(x, y, direction);
                }
            }
        }

        void Paint(int x, int y, Direction direction)
        {
            var newX = x + direction.X;
            var newY = y + direction.Y;

            if (IsOutOfBounds(newX, newY)) return;
            if (Equals(newGrid[newX, newY], '#')) return;

            if (newX != x)
            {
                newGrid[newX, newY] = 'O';
            }

            if (newY != y)
            {
                newGrid[newX, newY] = 'O';
            }
        }

        bool IsOutOfBounds(int newX, int newY)
        {
            if (newX < 0 || newX >= rows) return true;
            if (newY < 0 || newY >= columns) return true;

            return false;
        }
    }

    private void DebugPrint(char[,] grid)
    {
        for (int i = grid.GetLength(0) - 1; i >= 0; i--)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                Console.Write(grid[i, j]);
            }
            Console.WriteLine();
        }
    }

    private static char[,] DeepCleanCopyCharArray(char[,] original)
    {
        int rows = original.GetLength(0);
        int columns = original.GetLength(1);

        char[,] copy = new char[rows, columns];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                var originalChar = original[i, j];

                copy[i, j] = Equals(originalChar, 'S') ? '.' : originalChar;
            }
        }

        return copy;
    }

    private static char[,] ParseIntput()
    {
        var input = File.ReadAllLines("input.txt");

        int rows = input.Length;
        int cols = input[0].Length;

        char[,] grid = new char[rows, cols];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                grid[rows - 1 - i, j] = input[i][j];
            }
        }

        return grid;
    }
}
