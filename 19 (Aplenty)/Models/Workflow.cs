namespace Nineteen.Models;
public class Workflow
{
    public string Name { get; set; }
    public List<Instruction> Instructions { get; set; } = new List<Instruction>();
}
