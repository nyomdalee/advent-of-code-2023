namespace Fifteen;

public class Program
{
    public static void Main()
    {
        Console.WriteLine(GetSum());
    }

    private static long GetSum()
    {
        var line = File.ReadAllText("input.txt");
        var sequenceValues = line.Split(',');

        return sequenceValues.Select(GetReindeerHash).Sum();
    }

    private static int GetReindeerHash(string inputString)
    {
        int currentValue = 0;
        for (int i = 0; i < inputString.Length; i++)
        {
            var asciiValue = (int)inputString[i];
            currentValue += asciiValue;
            currentValue *= 17;
            currentValue %= 256;
        }
        return currentValue;
    }
}