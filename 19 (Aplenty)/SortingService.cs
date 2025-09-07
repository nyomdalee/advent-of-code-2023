using Nineteen.Models;

namespace Nineteen;
internal class SortingService
{
    private long total = 0;
    public long Go()
    {
        var workflows = ParseInput();
        var dictionary = workflows.ToDictionary(w => w.Name, w => w);

        var initialCandidate = new Candidate("in", 0)
        {
            Properties =
            [
                new("x", 1, 4000),
                new("m", 1, 4000),
                new("a", 1, 4000),
                new("s", 1, 4000),
            ],
        };

        var q = new Queue<Candidate>();
        q.Enqueue(initialCandidate);

        while (q.Count > 0)
        {
            HandleNext(q.Dequeue(), q, dictionary);
        }

        return total;
    }

    private void HandleNext(Candidate candidate, Queue<Candidate> q, Dictionary<string, Workflow> dict)
    {
        if (candidate.NextPipe == "R" || !IsValidCandidate(candidate))
        {
            return;
        }

        if (candidate.NextPipe == "A")
        {
            total += SumCandidate(candidate);
            return;
        }

        var wf = dict[candidate.NextPipe];
        var instruction = wf.Instructions[candidate.NextPipeInstruction];

        if (instruction.IsEnd)
        {
            q.Enqueue(candidate with { NextPipe = instruction.Result, NextPipeInstruction = 0 });
        }
        else
        {
            foreach (var branch in SplitCandidate(candidate, instruction))
            {
                q.Enqueue(branch);
            }
        }
    }

    private static IEnumerable<Candidate> SplitCandidate(Candidate candidate, Instruction instruction)
    {
        var prop = candidate.Properties.First(p => p.Name == instruction.Property);
        var others = candidate.Properties.Where(p => p.Name != instruction.Property).ToList();

        (Property passing, Property failing) = instruction.Comparison switch
        {
            ">" => (
                prop with { MinValue = Math.Max(prop.MinValue, instruction.Value + 1) },
                prop with { MaxValue = Math.Min(prop.MaxValue, instruction.Value) }),
            "<" => (
                prop with { MaxValue = Math.Min(prop.MaxValue, instruction.Value - 1) },
                prop with { MinValue = Math.Max(prop.MinValue, instruction.Value) }),
            _ => throw new InvalidOperationException($"Unknown comparison: {instruction.Comparison}")
        };

        yield return candidate with
        {
            Properties = [.. others, passing],
            NextPipe = instruction.Result,
            NextPipeInstruction = 0
        };

        yield return candidate with
        {
            Properties = [.. others, failing],
            NextPipeInstruction = candidate.NextPipeInstruction + 1
        };
    }

    private static bool IsValidCandidate(Candidate candidate) => candidate.Properties.All(x => x.MinValue <= x.MaxValue);

    private static long SumCandidate(Candidate candidate) => candidate.Properties.Aggregate(1L, (acc, p) => acc * (p.MaxValue - p.MinValue + 1));

    private static List<Workflow> ParseInput()
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

        return workflowList;
    }

    private static Workflow ParseWorkflow(string input)
    {
        if (string.IsNullOrEmpty(input)) throw new ArgumentException("Input cannot be null or empty");

        var workflow = new Workflow();

        var parts = input.Split('{');
        workflow.Name = parts[0];

        string instructionsPart = parts[1].TrimEnd('}');

        int index = 0;
        foreach (var instruction in instructionsPart.Split(','))
        {
            if (!instruction.Contains(':'))
            {
                workflow.Instructions.Add(new Instruction
                {
                    Index = index++,
                    Result = instruction,
                    IsEnd = true
                });
            }
            else
            {
                var finalSplit = instruction.Split(":");

                workflow.Instructions.Add(new Instruction
                {
                    Index = index++,
                    Property = finalSplit[0][..1],
                    Comparison = finalSplit[0][1..2],
                    Value = int.Parse(finalSplit[0][2..]),
                    Result = finalSplit[1],
                    IsEnd = false
                });
            }
        }
        return workflow;
    }
}