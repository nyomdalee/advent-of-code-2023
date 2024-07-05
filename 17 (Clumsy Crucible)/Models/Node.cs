using System.Drawing;
namespace Seventeen.Models;

internal record Node(int HeatLoss, Point Position, Direction Direction, int Streak, int RemainingEstimate)
{
    public int Heuristic => HeatLoss + RemainingEstimate;
}
