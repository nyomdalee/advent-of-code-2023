namespace Nineteen.Models;
public class Instruction
{
    public int Index { get; set; }
    public string Property { get; set; }
    public string Comparison { get; set; }
    public int Value { get; set; }
    public bool IsEnd { get; set; }
    public string Result { get; set; }
}
