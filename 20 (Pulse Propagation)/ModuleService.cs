using System.Diagnostics;
using Twenty.Models;
using Module = Twenty.Models.Module;

namespace Twenty;
internal class ModuleService
{
    public long Go()
    {
        var sw = new Stopwatch();
        sw.Start();

        Dictionary<string, Module> modules = ParseInput();

        var initialInstruction = new Instruction("", false, ["broadcaster"]);
        Queue<Instruction> queue = new(1000);

        var feederModule = modules.Single(x => x.Value.Targets.Contains("rx")).Value;
        Dictionary<string, long?> feederInputs = feederModule.InputHighPulse.Keys.ToDictionary(x => x, _ => default(long?));
        var feederModuleName = feederModule.Name;

        long i = 0;
        while (feederInputs.Values.Any(x => x is null))
        {
            i++;

            queue.Enqueue(initialInstruction);

            while (queue.Count > 0)
            {
                var instruction = queue.Dequeue();

                HandleInstruction(instruction);
            }
        }

        void HandleInstruction(Instruction instruction)
        {
            foreach (var target in instruction.Targets)
            {
                if (!modules.TryGetValue(target, out var mod))
                {
                    continue;
                }

                var result = mod.HandleInstruction(instruction);

                if (result is null) continue;

                if (result.IsHighPulse && result.Targets.Contains(feederModuleName))
                {
                    var srcName = result.Source;
                    if (feederInputs[srcName] is null)
                    {
                        feederInputs[srcName] = i;
                    }
                }

                queue.Enqueue(result);
            }
        }

        return Utils.Utils.LCM(feederInputs.Values.Select(x => x.Value).ToArray());
    }

    private Dictionary<string, Module> ParseInput()
    {
        var input = File.ReadAllLines("input.txt");

        if (input.Length == 0) throw new ArgumentException("Input cannot be null or empty");

        var modules = GetModules(input).ToList();
        PopulateInputs(modules);

        return modules.ToDictionary(x => x.Name, x => x);
    }

    private void PopulateInputs(IEnumerable<Module> modules)
    {
        foreach (var module in modules)
        {
            if (module.Type is not ModuleType.And) continue;

            module.PopulateInputs(modules);
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

            yield return new(name, [.. targets], type, false);
        }
    }

    private static ModuleType? GetModuleType(string moduleString) => moduleString switch
    {
        { } when moduleString.StartsWith('%') => ModuleType.Flop,
        { } when moduleString.StartsWith('&') => ModuleType.And,
        _ => null
    };
}
