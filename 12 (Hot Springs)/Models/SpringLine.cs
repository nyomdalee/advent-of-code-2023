namespace Twelve.Models;

internal class SpringLine(string text, int[] damagedGroups)
{
    public string Text { get; set; } = text;

    public int[] DamagedGroups { get; set; } = damagedGroups;
}
