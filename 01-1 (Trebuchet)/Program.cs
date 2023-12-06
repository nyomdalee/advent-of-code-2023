namespace OneOne;
public class Program
{
    public static void Main()
    {
        Console.WriteLine(GetSum());
    }

    private static int GetSum()
    {
        var lines = File.ReadAllLines($@"{AppDomain.CurrentDomain.BaseDirectory}\input.txt");
        int sum = 0;

        foreach (var line in lines)
        {
            int lineTotal = 0;
            int lastNumber = 0;
            foreach (var character in line)
            {
                if (char.IsNumber(character))
                {
                    lastNumber = int.Parse(character.ToString());
                    if (lineTotal == 0)
                        lineTotal = lastNumber * 10;
                }
            }
            lineTotal += lastNumber;
            sum += lineTotal;
        }
        return sum;
    }
}