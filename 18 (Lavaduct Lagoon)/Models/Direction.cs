namespace Eighteen.Models;
internal class Direction(int X, int Y, char Id)
{
    public char Id { get; set; } = Id;
    public int X { get; } = X;
    public int Y { get; } = Y;

    public override bool Equals(object? obj)
    {
        if (obj is not Direction dir)
            return false;

        return X == dir.X && Y == dir.Y;
    }
}
