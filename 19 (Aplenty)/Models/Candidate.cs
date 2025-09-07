namespace Nineteen.Models;

internal record Candidate(string NextPipe, int NextPipeInstruction)
{
    public List<Property> Properties { get; set; } = new List<Property>();
}
