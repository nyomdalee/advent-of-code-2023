using System.Text;

namespace Seven.Models;
internal class Hand
{
    public string Cards { get; }
    public int Bid { get; }
    public ulong Fitness { get; }

    //How not to write a ctor :)
    public Hand(string cards, int bid)
    {
        Cards = cards;
        Bid = bid;
        Fitness = CalculateFitness();
    }

    private Dictionary<string, string> _cardValues = new()
    {
        { "A", "01" }, { "K", "02" }, { "Q", "03" }, { "T", "04" }, { "9", "05" }, { "8", "06" }, { "7", "07" },
        { "6", "08" }, { "5", "09" }, { "4", "10" }, { "3", "11" }, { "2", "12" }, { "1", "13" }, { "J", "14" }
    };

    private ulong CalculateFitness()
    {
        var jCount = Cards.Where(c => c == 'J').Count();

        var builder = new StringBuilder();
        builder.Append((Cards.Distinct().Count() - (jCount > 0 ? jCount == 5 ? 0 : 1 : 0)).ToString());
        builder.Append(6 - (Cards
            .Where(c => c != 'J')
            .GroupBy(c => c)
            .Select(a => a.Count())
            .DefaultIfEmpty(0)
            .Max() + jCount)).ToString();

        foreach (var card in Cards)
        {
            builder.Append(_cardValues[card.ToString()]);
        }

        return ulong.Parse(builder.ToString());
    }
}