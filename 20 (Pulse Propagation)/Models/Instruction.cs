namespace Twenty.Models;

internal class Instruction(string source, bool isHighPulse, HashSet<string> targets)
{
    public string Source { get; init; } = source;
    public bool IsHighPulse { get; init; } = isHighPulse;
    public HashSet<string> Targets { get; init; } = targets;
}
