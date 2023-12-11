namespace Nine;

public class Program
{
    public static void Main()
    {
        Console.WriteLine(GetSum());
    }

    private static long GetSum()
    {
        var lines = File.ReadAllLines("input.txt");
        var histories = lines.Select(l => Array.ConvertAll(l.Split(' '), s => long.Parse(s)).ToList()).ToList();

        long sum = 0;

        foreach (var history in histories)
        {
            List<List<long>> layers = [new(history)];

            while (layers.Last().Any(n => n != 0))
            {
                List<long> newLayer = new();

                for (int j = 0; j < layers.Last().Count - 1; j++)
                {
                    newLayer.Add(layers.Last()[j + 1] - layers.Last()[j]);
                }
                layers.Add(newLayer);
            }

            layers.Reverse();
            layers[0].Add(0);

            for (int j = 1; j < layers.Count; j++)
            {
                layers[j].Add(layers[j].Last() + layers[j - 1].Last());
            }

            sum += layers[layers.Count - 1].Last();
        }
        return sum;
    }
}