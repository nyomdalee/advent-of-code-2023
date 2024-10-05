namespace Twenty.Models;

internal class Instruction(string source, bool isHighPulse, ICollection<string> targets)
{
    public string Source { get; init; } = source;
    public bool IsHighPulse { get; init; } = isHighPulse;
    public ICollection<string> Targets { get; init; } = targets;
}
