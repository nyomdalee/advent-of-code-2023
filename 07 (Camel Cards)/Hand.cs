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
        { "A", "01" }, { "K", "02" }, { "Q", "03" }, { "J", "04" }, { "T", "05" }, { "9", "06" }, { "8", "07" },
        { "7", "08" }, { "6", "09" }, { "5", "10" }, { "4", "11" }, { "3", "12" }, { "2", "13" }, { "1", "14" }
    };

    private ulong CalculateFitness()
    {
        var builder = new StringBuilder();
        builder.Append(Cards.Distinct().Count().ToString());
        builder.Append(6 - Cards.GroupBy(c => c).Select(a => a.Count()).Max()).ToString();

        foreach (var card in Cards)
        {
            builder.Append(_cardValues[card.ToString()]);
        }

        return ulong.Parse(builder.ToString());
    }
}