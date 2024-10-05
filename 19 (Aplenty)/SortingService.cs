using Nineteen.Models;

namespace Nineteen;
internal class SortingService
{
    public long Go()
    {
        var (workflows, materials) = ParseInput();

        var inWorkflow = workflows.Single(x => Equals(x.Name, "in"));

        long sum = 0;
        foreach (var mat in materials)
        {
            sum += HandleMaterial(mat, workflows, inWorkflow);
        }

        return sum;
    }

    private long HandleMaterial(Material mat, List<Workflow> workflows, Workflow inWorkflow)
    {
        var currentWorkflow = inWorkflow;
        while (true)
        {
            foreach (var instruction in currentWorkflow.Instructions)
            {
                if (IsCompliant(mat, instruction))
                {
                    if (Equals(instruction.Result, "R"))
                    {
                        return 0;
                    }

                    if (Equals(instruction.Result, "A"))
                    {
                        return SumMaterial(mat);
                    }

                    currentWorkflow = workflows.Single(x => Equals(x.Name, instruction.Result));
                    break;
                }
            }
        }
    }

    private long SumMaterial(Material mat) => mat.Properties.Sum(x => x.Value);

    private bool IsCompliant(Material mat, Instruction instruction)
    {
        if (string.IsNullOrWhiteSpace(instruction.Property))
        {
            return true;
        }

        var instructedProp = mat.Properties.Single(x => Equals(x.Name, instruction.Property));

        var propValue = instructedProp.Value;
        var insValue = instruction.Value;

        return Equals(instruction.Comparison, ">")
            ? propValue > insValue
            : propValue < insValue;
    }

    private (List<Workflow> workflowList, List<Material> materialList) ParseInput()
    {
        var input = File.ReadAllText("input.txt");

        if (string.IsNullOrEmpty(input)) throw new ArgumentException("Input cannot be null or empty");

        var sections = input.Split("\r\n\r\n");

        var workflowStrings = sections[0].Split("\r\n");

        List<Workflow> workflowList = [];
        foreach (var workflowString in workflowStrings)
        {
            workflowList.Add(ParseWorkflow(workflowString));
        }

        var materialStrings = sections[1].Split("\r\n");

        List<Material> materialList = [];
        foreach (var materialString in materialStrings)
        {
            materialList.Add(ParseMaterial(materialString));
        }

        return (workflowList, materialList);
    }

    private Workflow ParseWorkflow(string input)
    {
        if (string.IsNullOrEmpty(input)) throw new ArgumentException("Input cannot be null or empty");

        var workflow = new Workflow();

        var parts = input.Split('{');
        workflow.Name = parts[0];

        string instructionsPart = parts[1].TrimEnd('}');

        foreach (var instruction in instructionsPart.Split(','))
        {
            if (!instruction.Contains(':'))
            {
                workflow.Instructions.Add(new Instruction
                {
                    Result = instruction
                });
            }
            else
            {
                var finalSplit = instruction.Split(":");

                workflow.Instructions.Add(new Instruction
                {
                    Property = finalSplit[0][..1],
                    Comparison = finalSplit[0][1..2],
                    Value = int.Parse(finalSplit[0][2..]),
                    Result = finalSplit[1],
                });
            }
        }
        return workflow;
    }

    private Material ParseMaterial(string input)
    {
        if (string.IsNullOrEmpty(input)) throw new ArgumentException("Input cannot be null or empty");

        var material = new Material();

        string propertiesPart = input.Trim('{', '}');

        foreach (var propertyPair in propertiesPart.Split(','))
        {
            var parts = propertyPair.Split('=');

            material.Properties.Add(new Property
            {
                Name = parts[0],
                Value = int.Parse(parts[1])
            });
        }
        return material;
    }
}
