namespace Utils;

public static class Utils
{
    public static long LCM(params long[] values)
    {
        var primeFactors = values.Select(GetPrimeFactors);

        var commonFactors = primeFactors.Aggregate((a, b) => a.Intersect(b));
        var remainingFactors = primeFactors.SelectMany(f => f.Except(commonFactors));

        return commonFactors.Union(remainingFactors).Aggregate((a, b) => a * b);
    }

    static IEnumerable<long> GetPrimeFactors(long number)
    {
        List<long> factors = [];

        while (number % 2 == 0)
        {
            factors.Add(2);
            number /= 2;
        }

        for (long i = 3; i <= number; i += 2)
        {
            if (number == 1)
                break;

            while (number % i == 0)
            {
                factors.Add(i);
                number /= i;
            }
        }
        return factors;
    }
}
