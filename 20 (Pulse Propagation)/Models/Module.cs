namespace Twenty.Models;

internal class Module(string name, ICollection<string> targets, ModuleType? type, bool isOn)
{
    public string Name { get; init; } = name;
    public ICollection<string> Targets { get; init; } = targets;
    public ModuleType? Type { get; init; } = type;
    public bool IsOn { get; private set; } = isOn;
    public Dictionary<string, bool> InputHighPulse { get; set; } = [];

    public void PopulateInputs(IEnumerable<Module> modules)
    {
        var inputNames = modules.Where(x => x.Targets.Contains(Name)).Select(x => x.Name);
        InputHighPulse = inputNames.ToDictionary(x => x, _ => false);
    }

    public Instruction? HandleInstruction(Instruction instruction)
    {
        if (Type is ModuleType.Flop)
        {
            if (instruction.IsHighPulse)
            {
                return null;
            }

            IsOn = !IsOn;

            return new(Name, IsOn, Targets);
        }

        if (Type is ModuleType.And)
        {
            InputHighPulse[instruction.Source] = instruction.IsHighPulse;

            var sendHightPulse = !InputHighPulse.All(x => x.Value);

            return new(Name, sendHightPulse, Targets);
        }

        if (!Equals(Name, "broadcaster")) throw new Exception("untyped non broadcast module encountered");

        return new Instruction(Name, instruction.IsHighPulse, Targets);
    }
}
