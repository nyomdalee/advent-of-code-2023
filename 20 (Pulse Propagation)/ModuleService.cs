using Twenty.Models;
using Module = Twenty.Models.Module;

namespace Twenty;
internal class ModuleService
{
    public long Go()
    {
        var modules = ParseInput().ToList();

        var initialInstruction = new Instruction("", false, ["broadcaster"]);
        Queue<Instruction> queue = [];

        long lowCount = 0;
        long highCount = 0;

        for (int i = 0; i < 1000; i++)
        {
            queue.Enqueue(initialInstruction);

            while (queue.Count > 0)
            {
                var instruction = queue.Dequeue();

                if (instruction.IsHighPulse)
                {
                    highCount += instruction.Targets.Count;
                }
                else
                {
                    lowCount += instruction.Targets.Count;
                }

                HandleInstruction(instruction);
            }
        }

        void HandleInstruction(Instruction instruction)
        {
            foreach (var target in instruction.Targets)
            {
                var mod = modules.SingleOrDefault(x => Equals(x.Name, target));

                if (mod is null)
                {
                    continue;
                }

                var result = mod.HandleInstruction(instruction);

                if (result is not null)
                {
                    queue.Enqueue(result);
                }
            }
        }

        return lowCount * highCount;
    }

    private IEnumerable<Module> ParseInput()
    {
        var input = File.ReadAllLines("input.txt");

        if (input.Length == 0) throw new ArgumentException("Input cannot be null or empty");

        var modules = GetModules(input).ToList();
        PopulateInputs(modules);

        return modules;
    }

    private void PopulateInputs(IEnumerable<Module> modules)
    {
        foreach (var module in modules)
        {
            if (module.Type is not ModuleType.And) continue;

            module.PopulateInputs(modules);

            //var inputNames = modules.Where(x => x.Targets.Contains(module.Name)).Select(x => x.Name);
            //module.InputHighPulse = inputNames.ToDictionary(x => x, _ => false);
        }
    }

    private static IEnumerable<Module> GetModules(string[] input)
    {
        foreach (var line in input)
        {
            var sections = line.Split("->", StringSplitOptions.TrimEntries);

            var type = GetModuleType(sections[0]);
            var name = type is null ? sections[0] : sections[0][1..];
            var targets = sections[1].Split(',', StringSplitOptions.TrimEntries);

            yield return new(name, targets, type, false);
        }
    }

    private static ModuleType? GetModuleType(string moduleString) => moduleString switch
    {
        { } when moduleString.StartsWith('%') => ModuleType.Flop,
        { } when moduleString.StartsWith('&') => ModuleType.And,
        _ => null
    };
}
